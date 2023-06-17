using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Currencies;
using Gameplay.Orders;
using Gameplay.Services.Timer;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Infrastructure.YandexAds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Quests
{
    public class QuestsModel : IQuestModel
    {
        public event Action UpdateProgressDaily;
        public event Action ClearViewModel;
        public event Action<QuestsGuid, bool> IsRefreshQuest;
        public event Action UpdateProgressWeekly;

        private Dictionary<QuestsGuid, QuestComponent> WeeklyQuests = new();
        private Dictionary<QuestsGuid, QuestComponent> DailyQuests = new();
        private Dictionary<QuestsGuid, GameObject> RefreshDailyButtons = new();
        private Dictionary<QuestsGuid, GameObject> RefreshWeeklyButtons = new();
        private Dictionary<ResourceType, int> Resource = new();

        private const int CountDailyQuest = 3;
        private const int CountWeeklyQuest = 5;

        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private readonly CurrenciesModel _currenciesModel;
        private readonly TimerService _timerService;
        private readonly IWindowService _windowService;
        private readonly IAdsService _yandexAds;

        QuestsModel(IStaticDataService staticDataService, IProgressService progressService,
            CurrenciesModel currenciesModel, TimerService timerService, IWindowService windowService, IAdsService yandexAds)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _timerService = timerService;
            _windowService = windowService;
            _yandexAds = yandexAds;
        }

        public void StartSession()
        {
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].Stopped += DailyTimerStopped;

            if (_progressService.PlayerProgress.DailyQuests.Count == 0)
            {
                FillAvailableResource();
                ChooseDailyTasks();
                ChooseWeeklyTasks();
                UpdatingDataDay();
            }
            else
            {
                var lastSession = DateTime.Parse(_progressService.PlayerProgress.LastSession);
                var dateCreateWeek = DateTime.Parse(_progressService.PlayerProgress.DateCreateWeekQuests);

                if ((DateTime.Today - lastSession.Date).TotalDays >= 1)
                {
                    Resource.Clear();
                    _progressService.PlayerProgress.DailyQuests.Clear();
                    FillAvailableResource();
                    ChooseDailyTasks();
                }
                else                
                    LoadQuestProgress();

                if ((DateTime.Today - dateCreateWeek.Date).TotalDays >= 7)
                {
                    _progressService.PlayerProgress.WeeklyQuests.Clear();
                    ChooseWeeklyTasks();
                }
                else
                    LoadWeeklyQuestProgress();
                
            }
        }

        private void DailyTimerStopped(TimeModel model)
        {
            _progressService.PlayerProgress.DailyQuests.Clear();
            Resource.Clear();
            FillAvailableResource();
            ChooseDailyTasks();
            _windowService.Close(WindowType.DailyQuest);
        }

        private void LoadQuestProgress()
        {
            foreach (var questProgress in _progressService.PlayerProgress.DailyQuests)
            {
                var quest = _staticDataService.DailyQuestData.QuestComponents.FirstOrDefault(x
                    => x.QuestsGuid == questProgress.Guid);

                if (quest != null)
                {
                    DailyQuests.Add(quest.QuestsGuid, quest);
                    quest.QuestProgress = questProgress;
                }
            }
        }

        private void LoadWeeklyQuestProgress()
        {
            foreach (var questProgress in _progressService.PlayerProgress.WeeklyQuests)
            {
                var quest = _staticDataService.WeeklyQuestData.QuestComponents.FirstOrDefault(x
                    => x.QuestsGuid == questProgress.Guid);

                if (quest != null)
                {
                    WeeklyQuests.Add(quest.QuestsGuid, quest);
                    quest.QuestProgress = questProgress;
                }
            }
        }

        private void ChooseDailyTasks()
        {
            DailyQuests.Clear();
            var questAllDaily = _staticDataService.DailyQuestData.QuestComponents[0];

            if(questAllDaily.quest != QuestType.inaction)
                questAllDaily.Count = CreateCountProgressWeekly(Resource[questAllDaily.resource],questAllDaily.quest);
            questAllDaily.QuestProgress = _progressService.PlayerProgress.GetOrCreateQuestProgress(_progressService.PlayerProgress.DailyQuests
                ,questAllDaily.QuestsGuid, 0, false);
            DailyQuests.Add(questAllDaily.QuestsGuid, questAllDaily);
            
            for (var i = 0; i < CountDailyQuest; i++)
            {
                var quest = GetRandomUnassignedQuest(DailyQuests, true);

                if(quest.quest != QuestType.inaction)
                    quest.Count = CreateCountProgressDaily(Resource[quest.resource],quest.quest);
                
                quest.QuestProgress = _progressService.PlayerProgress.GetOrCreateQuestProgress(_progressService.PlayerProgress.DailyQuests
                    ,quest.QuestsGuid, 0, false);

                DailyQuests.Add(quest.QuestsGuid, quest);
            }
        }

        private void ChooseWeeklyTasks()
        {
            WeeklyQuests.Clear();
            var staticQuestWeekly = _staticDataService.WeeklyQuestData.QuestComponents[0];
            AddStaticWeeklyQuest(staticQuestWeekly);
            staticQuestWeekly = _staticDataService.WeeklyQuestData.QuestComponents.Find(x => x.QuestsGuid == QuestsGuid.doneDaily);
            AddStaticWeeklyQuest(staticQuestWeekly);

            for (var i = 0; i < CountWeeklyQuest; i++)
            {
                var quest = GetRandomUnassignedQuest(WeeklyQuests, false);

                if(quest.quest != QuestType.inaction)
                    quest.Count = CreateCountProgressWeekly(Resource[quest.resource],quest.quest);
                
                quest.QuestProgress = _progressService.PlayerProgress.GetOrCreateQuestProgress(_progressService.PlayerProgress.WeeklyQuests
                    ,quest.QuestsGuid, 0, false);

                WeeklyQuests.Add(quest.QuestsGuid, quest);
            }

            _progressService.PlayerProgress.DateCreateWeekQuests = DateTime.Today.ToString();
        }
        
        private void AddStaticWeeklyQuest(QuestComponent questAllWeekly)
        {
            if(questAllWeekly.quest != QuestType.inaction)
                questAllWeekly.Count = CreateCountProgressWeekly(Resource[questAllWeekly.resource],questAllWeekly.quest);
            
            questAllWeekly.QuestProgress = _progressService.PlayerProgress.GetOrCreateQuestProgress(_progressService.PlayerProgress.WeeklyQuests
                ,questAllWeekly.QuestsGuid, 0, false);
            WeeklyQuests.Add(questAllWeekly.QuestsGuid, questAllWeekly);
        }

        public void RefreshQuests(QuestsGuid id, bool isDaily)
        {
            if(_yandexAds.ShowAdsVideo());
                _yandexAds.VideoShown += () => RefreshAfterAds(id, isDaily);;
        }

        private void RefreshAfterAds(QuestsGuid id, bool isDaily)
        {
            IsRefreshQuest?.Invoke(id, isDaily);
        }

        public Dictionary<QuestsGuid, GameObject> GetRefreshButtons(bool isDaily)
        {
            if(isDaily)
                return RefreshDailyButtons;
            else
                return RefreshWeeklyButtons;
        }

        public void AddRefreshButtons(QuestsGuid guid, GameObject button, bool isDaily)
        {
            if (isDaily)
            {
                if (!RefreshDailyButtons.ContainsKey(guid))
                    RefreshDailyButtons.Add(guid, button);
            }
            else
            {
                if (!RefreshWeeklyButtons.ContainsKey(guid))
                    RefreshWeeklyButtons.Add(guid, button);
            }
        }

        public QuestComponent RefreshDailyData(QuestsGuid id)
        {
            var isDailyQuest = DailyQuests.ContainsKey(id);
            var isWeeklyQuest = WeeklyQuests.ContainsKey(id);
            if (!isDailyQuest && !isWeeklyQuest)
            {
                Debug.LogWarning("Происходит invoke для двух методов Week и Daily ");
                return null;
            }

            var quests = isDailyQuest ? DailyQuests : WeeklyQuests;

            if (isDailyQuest && _progressService.PlayerProgress.CountRefreshDaily == 0)
            {
                return null;
            }
            if (isWeeklyQuest && _progressService.PlayerProgress.CountRefreshWeekly == 0)
            {
                return null;
            }

            var quest = GetRandomUnassignedQuest(quests, isDailyQuest);
            
            var questProgressList = isDailyQuest ? _progressService.PlayerProgress.DailyQuests : _progressService.PlayerProgress.WeeklyQuests;
            
            if (isDailyQuest)
            {
                quest.QuestProgress = _progressService.PlayerProgress.GetOrCreateQuestProgress(
                    questProgressList, quest.QuestsGuid, 0, false);
                _progressService.PlayerProgress.RemoveQuest(id, _progressService.PlayerProgress.DailyQuests);
                _progressService.PlayerProgress.CountRefreshDaily--;
            }
            else
            {
                quest.QuestProgress = _progressService.PlayerProgress.GetOrCreateQuestProgress(
                    questProgressList, quest.QuestsGuid, 0, false);
                _progressService.PlayerProgress.RemoveQuest(id, _progressService.PlayerProgress.WeeklyQuests);
                _progressService.PlayerProgress.CountRefreshWeekly--;
            }

            quests.Remove(id);
            quests.Add(quest.QuestsGuid, quest);
            

            return quest;
        }
        private QuestComponent GetRandomUnassignedQuest(Dictionary<QuestsGuid, QuestComponent> quests, bool isDaily)
        {
            var unassignedQuests = GetUnassignedQuests(quests, isDaily);
            if (unassignedQuests.Count == 0)
            {
                return null;
            }
            return unassignedQuests[Random.Range(0, unassignedQuests.Count)];
        }

        private List<QuestComponent> GetUnassignedQuests(Dictionary<QuestsGuid, QuestComponent> quests, bool isDaily)
        {
            if (isDaily)
            {
                return _staticDataService.DailyQuestData.QuestComponents
                    .Where(q => !quests.ContainsKey(q.QuestsGuid) && !q.WIP).ToList();
            }
            else
            {
                return _staticDataService.WeeklyQuestData.QuestComponents
                    .Where(q => !quests.ContainsKey(q.QuestsGuid) && !q.WIP).ToList();
            }
        }

        public void TaskDailyProgress(QuestsGuid guid, int count)
        {
            if (DailyQuests.ContainsKey(guid))
            {
                if (DailyQuests[guid].QuestProgress.QuestProgress > DailyQuests[guid].Count)
                    DailyQuests[guid].QuestProgress.QuestProgress = DailyQuests[guid].Count;
                else
                {
                    if (DailyQuests[guid].QuestProgress.QuestProgress > DailyQuests[guid].Count)
                        DailyQuests[guid].QuestProgress.QuestProgress = DailyQuests[guid].Count;
                    DailyQuests[guid].QuestProgress.QuestProgress += count;
                    UpdateProgressDaily?.Invoke();
                }
            }
        }

        public void TaskWeeklyProgress(QuestsGuid guid, int count)
        {
            if (WeeklyQuests.ContainsKey(guid))
            {
                if (WeeklyQuests[guid].QuestProgress.QuestProgress > WeeklyQuests[guid].Count)
                    WeeklyQuests[guid].QuestProgress.QuestProgress = WeeklyQuests[guid].Count;
                else
                {
                    WeeklyQuests[guid].QuestProgress.QuestProgress += count;
                    if (WeeklyQuests[guid].QuestProgress.QuestProgress > WeeklyQuests[guid].Count)
                        WeeklyQuests[guid].QuestProgress.QuestProgress = WeeklyQuests[guid].Count;
                    UpdateProgressWeekly?.Invoke();
                }
            }
        }

        public void UpdatingDataDay()
        {
            _progressService.PlayerProgress.CountRefreshDaily = 3;
            _progressService.PlayerProgress.CountRefreshWeekly = 2;
        }

        public int GetCountRefreshQuests(bool isDaily)
        {
            if(isDaily)
                return _progressService.PlayerProgress.CountRefreshDaily;
            else
                return _progressService.PlayerProgress.CountRefreshWeekly;
            
        }

        public void TakeReward(int reward, QuestsGuid guid)
        {
            DailyQuests[guid].QuestProgress.IsTakeReward = true;
            _currenciesModel.Add(CurrencyType.SoftCurrency, reward);
            TaskDailyProgress(QuestsGuid.allDaily, 1);
            TaskWeeklyProgress(QuestsGuid.doneDaily, 1);
        }


        public void TakeWeeklyReward(int reward, QuestsGuid guid)
        {
            WeeklyQuests[guid].QuestProgress.IsTakeReward = true;
            _currenciesModel.Add(CurrencyType.SoftCurrency, reward);
            TaskWeeklyProgress(QuestsGuid.allWeekly, 1);
        }

        public Dictionary<QuestsGuid, QuestComponent> GetDailyQuests()
        {
            return DailyQuests;
        }

        public Dictionary<QuestsGuid, QuestComponent> GetWeeklyQuests()
        {
            return WeeklyQuests;
        }

        private void FillAvailableResource()
        {
            var processingProgresses =
                _progressService.RegionProgress.ProcessingWorkspaceProgresses.FindAll(x => x.IsBuilded);
            
            var miningProgresses =
                _progressService.RegionProgress.MiningWorkSpaceProgresses.FindAll(x => x.IsBuilded);
            
            foreach (var value in processingProgresses)
            {
                var processing = _staticDataService.GetProcessingWorkspaceData(value.Id);
                foreach (var resource in processing.BuildingLevels[value.CurrentLevel].ResourceConversionData
                             .OutputResources)
                {
                    Resource.Add(resource.ResourceType, resource.Value);
                }
            }

            foreach (var value in miningProgresses)
            {
                var mining = _staticDataService.MiningWorkspaceStaticData;
                if (Resource.ContainsKey(mining.ResourceType))
                {
                    Resource[mining.ResourceType] += mining.MinedResource;
                }
                else
                {
                    Resource.Add(mining.ResourceType, mining.MinedResource);
                }
            }
        }

        private int CreateCountProgressDaily(int count, QuestType type)
        {
            var createCount = 0;
            switch (type)
            {
                case QuestType.collect:
                    createCount = Random.Range(count * 2, count * 4);
                    break;
                case QuestType.process:
                    createCount = Random.Range(count * 3, count * 6);
                    break;
                case QuestType.inaction:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            return createCount;
        }
        
        private int CreateCountProgressWeekly(int count, QuestType type)
        {
            var createCount = 0;
            switch (type)
            {
                case QuestType.collect:
                    createCount = Random.Range(count * 13, count * 21);
                    break;
                case QuestType.process:
                    createCount = Random.Range(count * 15, count * 26);
                    break;
                case QuestType.inaction:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            return createCount;
        }


    }
}