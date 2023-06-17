using Gameplay.CameraProject;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Region;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.StaticData;
using Infrastructure.StaticData.ProcessingWorkspace;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Workspaces
{
    public class WorkspaceFactory : IWorkspaceFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private readonly ICoroutineService _coroutineService;
        private readonly IWindowService _windowService;
        private readonly CurrenciesModel _currenciesModel;
        private readonly IQuestModel _questModel;
        private readonly TimerService _timerService;
        private readonly CameraZoomController _zoomController;

        private IRegionStorage _regionStorage;

        public WorkspaceFactory(IStaticDataService staticDataService, IProgressService progressService, ICoroutineService coroutineService,
            IWindowService windowService, CurrenciesModel currenciesModel, IQuestModel questModel, TimerService timerService, CameraZoomController zoomController)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _coroutineService = coroutineService;
            _windowService = windowService;
            _currenciesModel = currenciesModel;
            _questModel = questModel;
            _timerService = timerService;
            _zoomController = zoomController;
        }

        public void Initialize(IRegionStorage regionStorage)
        {
            _regionStorage = regionStorage;
        }

        public MiningWorkSpaceModel CreateMiningWorkSpaceModel(Vector3Int guid)
        {
            var progress = _progressService.RegionProgress.GetOrCreateMiningWorkspaceProgress(guid);
            var data = _staticDataService.MiningWorkspaceStaticData;
            MiningWorkSpaceModel model = new(progress, data, _currenciesModel);
            
            NotBuildedState miningNotBuildedState = new MiningNotBuildedState(_windowService, model, _zoomController);
            var constructionState = new ConstructionState(progress, data, _coroutineService, model);
            WorkingState miningWorkingState = new MiningWorkingState(_coroutineService, data, _regionStorage, _windowService, model, _questModel);
            IdleState idleState = new MiningIdleState();
            var miningStateMachine = new StateMachine();
            
            miningStateMachine.AddState(miningNotBuildedState);
            miningStateMachine.AddState(constructionState);
            miningStateMachine.AddState(miningWorkingState);
            miningStateMachine.AddState(idleState);
            
            model.Initialize(miningStateMachine);
            return model;
        }

        public ProcessingWorkspaceModel CreateProcessingWorkspaceModel(ProcessingType id, Vector3Int guid)
        {
            var progress =
                _progressService.RegionProgress.GetOrCreateProcessingWorkspaceProgress(guid, id);
            var data = _staticDataService.GetProcessingWorkspaceData(id);
            
            
            var model = new ProcessingWorkspaceModel(progress, data, _currenciesModel, _staticDataService, _questModel);

            var stateMachine = new StateMachine();
            
            NotBuildedState processNotBuildedState = new ProcessNotBuildedState(_windowService, model, _zoomController);
            IdleState idleState = new ProcessingIdleState(model, _windowService);
            WorkingState workingState = new ProcessingWorkingState(_windowService, model, _timerService);
            var constructionState = new ConstructionState(progress, data, _coroutineService, model);
            
            stateMachine.AddState(processNotBuildedState);
            stateMachine.AddState(idleState);
            stateMachine.AddState(workingState);
            stateMachine.AddState(constructionState);
            
            model.Initialize(stateMachine);
            return model;
        }

        public StorageOilCrudeModel CreateStorageOilCrudeWorkspaceModel(Vector3Int guid)
        {
            var progress = _progressService.RegionProgress.GetOrCreateStorageOilCrudeProgress(guid);
            var data = _staticDataService.StorageOilCrudeStaticData;
            var model = new StorageOilCrudeModel(progress, data, _currenciesModel, _regionStorage, _questModel);
            
            var stateMachine = new StateMachine();
            
            NotBuildedState notBuildedState = new OilStorageNotBuildedState(_windowService, model, _zoomController);
            var idleState = new IdleState();
            var workingState = new WorkingState();
            var constructionState = new ConstructionState(progress, data, _coroutineService, model);
            
            stateMachine.AddState(notBuildedState);
            stateMachine.AddState(idleState);
            stateMachine.AddState(workingState);
            stateMachine.AddState(constructionState);
            
            model.Initialize(stateMachine);
            return model;
        }

        public ComplexWorkspaceModel CreateComplexWorkspaceModel(Vector3Int guid, ComplexType complexType)
        {
            var progress = _progressService.RegionProgress.GetOrCreateComplexWorkspaceProgress(guid);
            var data = _staticDataService.GetComplexWorkspaceStaticData(complexType);
            var model = new ComplexWorkspaceModel(progress, data, _currenciesModel, _regionStorage, _questModel);
            
            var stateMachine = new StateMachine();
            
            NotBuildedState notBuildedState = new ComplexNotBuildedState(_windowService, model, _zoomController);
            IdleState idleState = new ComplexIdleState();
            WorkingState workingState = new ComplexWorkingState(_windowService, model);
            var constructionState = new ConstructionState(progress, data, _coroutineService, model);
            
            stateMachine.AddState(notBuildedState);
            stateMachine.AddState(idleState);
            stateMachine.AddState(workingState);
            stateMachine.AddState(constructionState);
            model.Initialize(stateMachine);
            return model;
        }
    }
}