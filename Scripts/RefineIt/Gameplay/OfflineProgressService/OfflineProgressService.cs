using System;
using System.Linq;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Gameplay.Workspaces;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace Gameplay.OfflineProgressService
{
    public class OfflineProgressService : MonoBehaviour
    {
        public int TimeElapsed { get; private set; }

        private IProgressService _progressService;
        private IWorkspaceService _workspaceService;
        private IRegionStorage _regionStorage;
        private IStaticDataService _staticDataService;
        private TimerService _timerService;
        
        private DateTime _focusTime = DateTime.Now;
        private DateTime _notFocusTime = DateTime.Now;

        [Inject]
        public void Construct(IProgressService progressService, IWorkspaceService workspaceService,
            IRegionStorage regionStorage, IStaticDataService staticDataService, TimerService timerService)
        {
            _workspaceService = workspaceService;
            _regionStorage = regionStorage;
            _staticDataService = staticDataService;
            _progressService = progressService;
            _timerService = timerService;
        }

        public void Initialization()
        {
            InitializeDailyTimer();
            
            if (_progressService.PlayerProgress.LastSession == null)
            {
                return;
            }

            var datetime = DateTime.Parse(_progressService.PlayerProgress.LastSession);
            TimeElapsed = (int)(DateTime.Now - datetime).TotalSeconds;

            BuildingProcess();
        }

        public void InitializeProcessingProgress()
        {
            if (_progressService.PlayerProgress.LastSession != null)
            {
                MiningOfflineProgress();
                ProcessingOfflineProgress();
                OrderTimer();
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                _notFocusTime = DateTime.Now;
            }
            else
            {
                _focusTime = DateTime.Now;

                if (_focusTime > _notFocusTime.AddSeconds(1))
                {
                    TimeElapsed = (int)(_focusTime - _notFocusTime).TotalSeconds;
                    
                    BuildingProcess();
                    InitializeProcessingProgress();
                    DailyTimer();
                }
            }
        }

        private void OrderTimer()
        {
            foreach (var timeModel in _timerService.TimeModels.Values)
            {
                timeModel.TimeProgress.Time -= TimeElapsed;
            }

            foreach (var orderProgress in _progressService.RegionProgress.OrdersProgress.OrderProgresses
                         .Where(orderProgress => orderProgress.OrderStatus == OrderStatus.Working))
            {
                orderProgress.Time -= TimeElapsed;
            }
        }

        private void MiningOfflineProgress()
        {
            foreach (var model in _workspaceService.MiningWorkspaces.Values)
            {
                if (model.IsBuilded)
                {
                    var duration = TimeElapsed / (int)_staticDataService.MiningWorkspaceStaticData.MiningDuration;
                    var bonusMinedResource = model.GetResourceBonus(_staticDataService.MiningWorkspaceStaticData.ResourceType);
                    var minedResource = duration * _staticDataService.MiningWorkspaceStaticData.MinedResource;
                    _regionStorage.AddResources(ResourceType.Oil, (int)(minedResource * bonusMinedResource));
                }
            }
        }

        private void ProcessingOfflineProgress()
        {
            foreach (var model in _workspaceService.ProcessingWorkspaces.Values)
            {
                if (model.IsBuilded)
                {
                    var transportTime = 0;
                    transportTime = GetTransportPathTime(model, transportTime);

                    var duration = TimeElapsed / (model.CurrentLevelData.ProcessingTime + transportTime);

                    for (var i = 0; i < duration; i++)
                    {
                        if (CheckCanTakeResources(model))
                        {
                            foreach (var resource in model.CurrentLevelData.ResourceConversionData
                                         .InputResources)
                            {
                                _regionStorage.TakeResources(resource.ResourceType, resource.Value);
                            }

                            foreach (var resource in model.CurrentLevelData.ResourceConversionData
                                         .OutputResources)
                            {
                                var processingBonus = model.GetResourceBonus(resource.ResourceType);
                                _regionStorage.AddResources(resource.ResourceType, (int)(resource.Value * processingBonus));
                            }
                        }
                    }
                }
            }
        }

        private int GetTransportPathTime(ProcessingWorkspaceModel model, int transportTime)
        {
            foreach (var transportStaticData in _staticDataService.TransportsData.Values)
            {
                var transportPathData = _staticDataService.TransportPathsStaticData.Paths.Find(x =>
                    x.TransportType == transportStaticData.Type);

                var progress = _progressService.RegionProgress.TransportProgresses.Find(x =>
                    x.TransportType == transportPathData?.TransportType);

                if (progress != null)
                {
                    string transportId = null;
                    foreach (var resource in model.CurrentLevelData.ResourceConversionData
                                 .OutputResources)
                    {
                        transportId += $"{resource.ResourceType}";
                    }

                    if (transportId == transportStaticData.Type.ToString())
                    {
                        transportTime += (int)transportStaticData.Levels[progress.CurrentLevel].ShippingTime;
                    }
                }
            }

            return transportTime;
        }

        private bool CheckCanTakeResources(ProcessingWorkspaceModel model)
        {
            var inputResourcesData = model.CurrentLevelData.ResourceConversionData.InputResources;

            return inputResourcesData.All(inputResource =>
                model.InputResourceStorage.CanTakeResources(inputResource.ResourceType, inputResource.Value));
        }

        private void BuildingProcess()
        {
            ComplexConstructionProgress();
            ProcessingConstructionProgress();
            MiningConstructionProgress();
        }

        private void ComplexConstructionProgress()
        {
            foreach (var buildingData in _progressService.RegionProgress.ComplexWorkspaceProgresses)
            {
                if (buildingData.ConstructionProgress > 0 && !buildingData.IsBuilded)
                {
                    var constructionProgress = TimeElapsed / _staticDataService
                        .GetComplexWorkspaceStaticData(buildingData.ComplexType)
                        .BuildTime + buildingData.ConstructionProgress;

                    buildingData.ConstructionProgress = constructionProgress;

                    if (constructionProgress >= 1)
                    {
                        buildingData.IsBuilded = true;
                    }
                }
            }
        }

        private void ProcessingConstructionProgress()
        {
            foreach (var buildingData in _progressService.RegionProgress.ProcessingWorkspaceProgresses)
            {
                if (buildingData.ConstructionProgress > 0 && !buildingData.IsBuilded)
                {
                    var constructionProgress = TimeElapsed / _staticDataService
                        .GetProcessingWorkspaceData(buildingData.Id).BuildTime + buildingData.ConstructionProgress;

                    buildingData.ConstructionProgress = constructionProgress;

                    if (constructionProgress >= 1)
                    {
                        buildingData.IsBuilded = true;
                    }
                }
            }
        }

        private void MiningConstructionProgress()
        {
            foreach (var buildingData in _progressService.RegionProgress.MiningWorkSpaceProgresses)
            {
                if (buildingData.ConstructionProgress > 0 && !buildingData.IsBuilded)
                {
                    var constructionProgress = TimeElapsed / _staticDataService
                        .MiningWorkspaceStaticData.BuildTime + buildingData.ConstructionProgress;

                    buildingData.ConstructionProgress = constructionProgress;

                    if (constructionProgress >= 1)
                    {
                        buildingData.IsBuilded = true;
                    }
                }
            }
        }

        private void DailyTimer()
        {
            if(_timerService.TimeModels.ContainsKey(TimerType.DailyTimer.ToString()))
                _timerService.TimeModels[TimerType.DailyTimer.ToString()].TimeProgress.Time -= TimeElapsed;
        }

        private void CreateDailyTimer(TimeModel model)
        {
            _timerService.CreateTimer(TimerType.DailyTimer.ToString(), 86400);
        }

        private void InitializeDailyTimer()
        {
            var time = FormatTime.HoursIntFormat(DateTime.Now.Hour) 
                       + FormatTime.MinutesIntFormat(DateTime.Now.Minute)
                       + DateTime.Now.Second;
            _timerService.CreateTimer(TimerType.DailyTimer.ToString(), 86400);
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].TimeProgress.Time -= time;
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].Stopped += CreateDailyTimer;

        }
    }
}