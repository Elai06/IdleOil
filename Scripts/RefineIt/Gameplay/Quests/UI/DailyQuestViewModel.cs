using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.Quests.UI
{
    public class DailyQuestViewModel : ViewModelBase<IQuestModel, DailyQuestView>
    {
        public DailyQuestViewModel(IQuestModel model, DailyQuestView view) : base(model, view)
        {
        }

        public override Task Show()
        {
            ShowDailySubView();
            HideRefreshButton();
            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            Model.IsRefreshQuest += IsRefreshSubView;
            Model.IsRefreshQuest += HideIsRefreshButtons;
            Model.UpdateProgressDaily += UpdateSubViews;
            Model.ClearViewModel += ClearContainer;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            Model.IsRefreshQuest -= IsRefreshSubView;
            Model.IsRefreshQuest -= HideIsRefreshButtons;
            Model.UpdateProgressDaily -= UpdateSubViews;
            Model.ClearViewModel -= ClearContainer;
            UnsubscribeButton();
        }

        private void ShowDailySubView()
        {  
            View.DailyQuestContainer.CleanUp();
            var quests = Model.GetDailyQuests();
                                
            foreach (var quest in quests)
            {
                var description = CreateDescription(quest.Value.description, quest.Value.Count);
                var data = new QuestSubData(quest.Value.Sprite, description, quest.Key
                    , quest.Value.Reward.ToString(), quest.Value.Count, quest.Value.QuestProgress.QuestProgress, true);

                View.DailyQuestContainer.Add(quest.Key.ToString(), data);

                if (quest.Value.QuestProgress.QuestProgress >= quest.Value.Count)
                {
                    View.DailyQuestContainer.SubViews[quest.Key.ToString()].ActiveRewardButton(true);
                    View.DailyQuestContainer.SubViews[quest.Key.ToString()].HideRefreshButton();
                }

                if (quest.Value.QuestProgress.IsTakeReward)
                {
                    View.DailyQuestContainer.SubViews[quest.Key.ToString()].HideRewardButton();
                }

                View.DailyQuestContainer.SubViews[quest.Key.ToString()].ButtonTake.ClickButton += Model.TakeReward;
                View.DailyQuestContainer.SubViews[quest.Key.ToString()].ButtonAds.ClickButton += Model.RefreshQuests;
                
                var refreshBtn = View.DailyQuestContainer.SubViews[quest.Key.ToString()].GetRefreshGameObject();
                Model.AddRefreshButtons(quest.Key, refreshBtn, true);
            }
        }

        private void UnsubscribeButton()
        {
            foreach (var daily in View.DailyQuestContainer.SubViews.Values)
            {
                daily.ButtonTake.ClickButton -= Model.TakeReward;
                daily.ButtonAds.ClickButton -= Model.RefreshQuests;
            }
        }

        private void UpdateSubViews()
        {
            var quests = Model.GetDailyQuests();
            foreach (var subView in View.DailyQuestContainer.SubViews)
            {
                var subGuid = (QuestsGuid)Enum.Parse(typeof(QuestsGuid), subView.Key);   
                var quest = quests[subGuid];
                var description = CreateDescription(quest.description, quest.Count);

                var data = new QuestSubData(quest.Sprite, description, quest.QuestsGuid
                    , quest.Reward.ToString(), quest.Count, quest.QuestProgress.QuestProgress, true);

                View.DailyQuestContainer.UpdateView(data, subView.Key);
                if (quest.QuestProgress.QuestProgress >= quest.Count)
                {
                    View.DailyQuestContainer.SubViews[subView.Key].ActiveRewardButton(true);
                    View.DailyQuestContainer.SubViews[subView.Key].HideRefreshButton();
                }
            }
        }

        private void IsRefreshSubView(QuestsGuid guid, bool isDaily)
        {
            if (!isDaily) return;
            
            var quest = Model.RefreshDailyData(guid);
            
            if (quest == null) return;
                    
            var description = CreateDescription(quest.description, quest.Count);
            var data = new QuestSubData(quest.Sprite, description, quest.QuestsGuid
                , quest.Reward.ToString(), quest.Count, quest.QuestProgress.QuestProgress, true);

            Model.GetRefreshButtons(true).Remove(guid);
            View.DailyQuestContainer.Remove(guid.ToString());

            View.DailyQuestContainer.Add(quest.QuestsGuid.ToString(), data);
            View.DailyQuestContainer.SubViews[quest.QuestsGuid.ToString()].ButtonAds.ClickButton += Model.RefreshQuests;
            var refreshBtn = View.DailyQuestContainer.SubViews[quest.QuestsGuid.ToString()].GetRefreshGameObject();
            Model.AddRefreshButtons(quest.QuestsGuid, refreshBtn, true);
        }

        private void HideIsRefreshButtons(QuestsGuid guid, bool isDaily)
        {
            if (Model.GetCountRefreshQuests(true) <= 0)
            {
                var buttons = Model.GetRefreshButtons(true);
                foreach (var button in buttons)
                {
                    button.Value.SetActive(false);
                }
            }
        }

        private void HideRefreshButton()
        {
            var buttons = Model.GetRefreshButtons(true);
            buttons[QuestsGuid.allDaily].gameObject.SetActive(false);
            
            if (Model.GetCountRefreshQuests(true) <= 0)
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

        public void ClearContainer()
        {
            View.DailyQuestContainer.CleanUp();
        }
    }
}