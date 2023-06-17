using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.Quests.UI
{
    public class WeeklyQuestViewModel : ViewModelBase<IQuestModel, WeeklyQuestView>
    {
        public WeeklyQuestViewModel(IQuestModel model, WeeklyQuestView view) : base(model, view)
        {
            
        }

        public override Task Show()
        {
            ShowWeeklySubView();
            HideRefreshButtons(QuestsGuid.allDaily,false);
            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            Model.IsRefreshQuest += RefreshWeeklySubView;
            Model.IsRefreshQuest += HideRefreshButtons;
            Model.UpdateProgressWeekly += UpdateSubViews;
        }

        public override void Unsubscribe()
        {
            Model.IsRefreshQuest -= RefreshWeeklySubView;
            Model.IsRefreshQuest -= HideRefreshButtons;
            Model.UpdateProgressWeekly -= UpdateSubViews;
            UnsubscribeButton();
        }

        private void ShowWeeklySubView()
        {
            var quests = Model.GetWeeklyQuests();
                                
            foreach (var quest in quests)
            {
                var description = CreateDescription(quest.Value.description, quest.Value.Count);
                var data = new QuestSubData(quest.Value.Sprite, description, quest.Key
                    , quest.Value.Reward.ToString(), quest.Value.Count, quest.Value.QuestProgress.QuestProgress, false);

                View.WeeklyQuestContainer.Add(quest.Key.ToString(), data);

                if (quest.Value.QuestProgress.QuestProgress >= quest.Value.Count)
                {
                    View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].ActiveRewardButton(true);
                    View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].HideRefreshButton();
                }

                if (quest.Value.QuestProgress.IsTakeReward)
                {
                    View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].HideRewardButton();
                }

                View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].ButtonTake.ClickButton += Model.TakeWeeklyReward;
                View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].ButtonAds.ClickButton += Model.RefreshQuests;
                
                var refreshBtn = View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].GetRefreshGameObject();
                Model.AddRefreshButtons(quest.Key, refreshBtn, false);
            }
        }

        private void UnsubscribeButton()
        {
            foreach (var daily in View.WeeklyQuestContainer.SubViews.Values)
            {
                daily.ButtonTake.ClickButton -= Model.TakeWeeklyReward;
                daily.ButtonAds.ClickButton -= Model.RefreshQuests;
            }
        }

        private void UpdateSubViews()
        {
            var quests = Model.GetWeeklyQuests();
            foreach (var subView in View.WeeklyQuestContainer.SubViews)
            {
                var subGuid = (QuestsGuid)Enum.Parse(typeof(QuestsGuid), subView.Key);   
                var quest = quests[subGuid];
                var description = CreateDescription(quest.description, quest.Count);

                var data = new QuestSubData(quest.Sprite, description, quest.QuestsGuid
                    , quest.Reward.ToString(), quest.Count, quest.QuestProgress.QuestProgress, false);

                View.WeeklyQuestContainer.UpdateView(data, subView.Key);
                if (quest.QuestProgress.QuestProgress >= quest.Count)
                {
                    View.WeeklyQuestContainer.SubViews[subView.Key].ActiveRewardButton(true);
                    View.WeeklyQuestContainer.SubViews[subView.Key].HideRefreshButton();
                }
            }
        }

        private void RefreshWeeklySubView(QuestsGuid guid, bool isDaily)
        {
            if(isDaily) return;
            
            var quest = Model.RefreshDailyData(guid);
            
            if(quest == null) return;
            
            var description = CreateDescription(quest.description, quest.Count);
            var data = new QuestSubData(quest.Sprite, description, quest.QuestsGuid
                , quest.Reward.ToString(), quest.Count, quest.QuestProgress.QuestProgress, false);

            Model.GetRefreshButtons(false).Remove(guid);
            View.WeeklyQuestContainer.Remove(guid.ToString());
            
            View.WeeklyQuestContainer.Add(quest.QuestsGuid.ToString(), data);
            View.WeeklyQuestContainer.SubViews[quest.QuestsGuid.ToString()].ButtonAds.ClickButton += Model.RefreshQuests;
            var refreshBtn = View.WeeklyQuestContainer.SubViews[quest.QuestsGuid.ToString()].GetRefreshGameObject();
            Model.AddRefreshButtons(quest.QuestsGuid, refreshBtn, false);
        }

        private void HideRefreshButtons(QuestsGuid guid, bool isDaily)
        {
            var buttons = Model.GetRefreshButtons(false);
            buttons[QuestsGuid.allWeekly].gameObject.SetActive(false);
            buttons[QuestsGuid.doneDaily].gameObject.SetActive(false);
            
            if (Model.GetCountRefreshQuests(false) <= 0)
            {
                
                foreach (var button in buttons)
                {
                    button.Value.SetActive(false);
                }
            }
        }

        private string CreateDescription(string description, int count)
        {
            return description.Replace("[N]", count.ToString());
        }
    }
}