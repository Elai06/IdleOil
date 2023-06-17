using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Workspaces;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using UnityEngine;

namespace Gameplay.Workers
{
    public class WorkersService
    {
        public event Action<string> Upgrade;

        public readonly Dictionary<string, WorkerModel> WorkerModels = new();

        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private readonly CurrenciesModel _currenciesModel;
        private readonly IWorkspaceService _workspaceService;
        private readonly IQuestModel _questModel;

        public WorkersService(IStaticDataService staticDataService, IProgressService progressService,
            CurrenciesModel currenciesModel, IWorkspaceService workspaceService, IQuestModel questModel)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _workspaceService = workspaceService;
            _questModel = questModel;
        }

        public void Initialize()
        {
            foreach (var personnelData in _staticDataService.WorkersConfig.WorkersData)
            {
                var workerProgress = _progressService.RegionProgress
                    .GetOrCreatePersonnelProgress(personnelData.WorkerType);

                var model = CreatePersonnelModel(personnelData, workerProgress);

                WorkerModels.Add(personnelData.WorkerType.ToString(), model);

                if (workerProgress.IsBuy)
                {
                    AddProcessingBonusResource(workerProgress.WorkerType.ToString());
                    AddMiningBonusResource(workerProgress.WorkerType.ToString());
                }
            }
        }

        public void UpgradeWorkerLevel(string id)
        {
            if (WorkerModels[id].BuyOrUpgrade())
            {
                Upgrade?.Invoke(id);
                _questModel.TaskDailyProgress(QuestsGuid.updatePersonal,1);
                _questModel.TaskWeeklyProgress(QuestsGuid.updatePersonalWeek,1);
                AddProcessingBonusResource(id);
                AddMiningBonusResource(id);
            }
        }

        private void AddProcessingBonusResource(string id)
        {
            foreach (var model in _workspaceService.ProcessingWorkspaces.Values)
            {
                foreach (var resource in model.CurrentLevelData
                             .ResourceConversionData.OutputResources)
                {
                    if (resource.ResourceType == WorkerModels[id].WorkerData.ResourceType)
                    {
                        model.AddProcessingBonus(resource.ResourceType, WorkerModels[id].CurrentLevelData.Bonus);
                    }
                }
            }
        }

        private void AddMiningBonusResource(string id)
        {
            foreach (var model in _workspaceService.MiningWorkspaces.Values)
            {
                var data = _staticDataService.MiningWorkspaceStaticData;
                if (data.ResourceType == WorkerModels[id].WorkerData.ResourceType)
                {
                    model.AddMiningBonus(data.ResourceType, WorkerModels[id].CurrentLevelData.Bonus);
                }
            }
        }

        private WorkerModel CreatePersonnelModel(WorkerData workerData, WorkerProgress workerProgress)
        {
            return new WorkerModel(workerData, workerProgress, _currenciesModel);
        }
    }
}