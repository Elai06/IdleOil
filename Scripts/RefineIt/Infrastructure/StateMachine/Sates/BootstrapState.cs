using Gameplay.Region;
using Gameplay.Region.Data;
using Gameplay.Region.Storage;
using Gameplay.Workspaces;
using Infrastructure.PersistenceProgress;
using Infrastructure.SaveLoads;
using Infrastructure.StaticData;

namespace Infrastructure.StateMachine.Sates
{
    public class BootstrapState : IPayloadedState<RegionType>
    {
        private IStateMachine _stateMachine;
        private readonly IWorkspaceFactory _workspaceFactory;
        private readonly IRegionStorage _regionStorage;
        private readonly IStaticDataService _staticDataService;
        private readonly IRegionFactory _regionFactory;
        private RegionModel _regionModel;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;

        public BootstrapState(IWorkspaceFactory workspaceFactory, IRegionStorage regionStorage,
            IStaticDataService staticDataService, IRegionFactory regionFactory, ISaveLoadService saveLoadService,
            IProgressService progressService)
        {
            _workspaceFactory = workspaceFactory;
            _regionStorage = regionStorage;
            _staticDataService = staticDataService;
            _regionFactory = regionFactory;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Exit()
        {
        }

        public void Enter(RegionType regionType)
        {
            InitializeDependencies();
            _saveLoadService.Load();
            _staticDataService.Load();
            _staticDataService.LoadRegionConfigs();
            SelectRegion(regionType);
            _regionModel = _regionFactory.CreateSelectedRegionModel();

            _stateMachine.Enter<LoadLevelState, RegionModel>(_regionModel);
        }

        private void SelectRegion(RegionType regionType)
        {
            foreach (var regionProgress in _progressService.PlayerProgress.RegionProgresses)
            {
                regionProgress.IsSelected = regionProgress.RegionType == regionType;
            }
        }

        private void InitializeDependencies()
        {
            _workspaceFactory.Initialize(_regionStorage);
        }
    }
}