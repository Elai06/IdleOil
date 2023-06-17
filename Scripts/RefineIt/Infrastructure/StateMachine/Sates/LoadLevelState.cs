using Gameplay.CameraProject;
using Gameplay.Currencies;
using Gameplay.DailyEntry;
using Gameplay.OfflineProgressService;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Personnel;
using Gameplay.PromoCode;
using Gameplay.Quests;
using Gameplay.Region;
using Gameplay.Region.Storage;
using Gameplay.Settings;
using Gameplay.Shop;
using Gameplay.Store;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Factories;
using Gameplay.Tilemaps.Services;
using Gameplay.Workers;
using Gameplay.Workspaces.Workers.Pipeline;
using Gameplay.Workspaces.Workers.Transport;
using Gameplay.Workspaces.Workers.Wagon;
using Infrastructure.SceneManagement;
using Infrastructure.StaticData;

namespace Infrastructure.StateMachine.Sates
{
    public class LoadLevelState : IPayloadedState<RegionModel>
    {
        private const string Scene = "GameNewLocation";

        private readonly IRegionStorage _regionStorage;
        private readonly IStaticDataService _staticDataService;
        private readonly CurrenciesModel _currenciesModel;
        private readonly ISceneLoader _sceneLoader;
        private readonly ITileClickHandler _tileClickHandler;
        private readonly ITileViewFactory _tileViewFactory;
        private readonly ITilemapController _tilemapController;
        private readonly ITileSpawner _tileSpawner;
        private readonly ITileStorage _tileStorage;
        private readonly ITransportSpawner _transportSpawner;
        private readonly ITransportOrderSpawner _transportOrderSpawner;
        private readonly IPipelineSpawner _pipelineSpawner;
        private readonly OrderGenerateService _orderGenerateService;
        private readonly OrderTutorialGenerateService _orderTutorialGenerateService;
        private readonly IQuestModel _questModel;
        private readonly ISettingModel _settingModel;
        private readonly AudioService _audioService;

        private readonly OfflineProgressService _offlineProgressService;
        private readonly PromoCodeModel _promoCodeModel;
        private readonly WorkersService _workersService;
        private readonly CameraZoomController _cameraZoom;
        private readonly IShopModel _shopModel;
        private readonly IDailyEntryModel _dailyEntryModel;

        private IStateMachine _stateMachine;
        private RegionModel _regionModel;

        public LoadLevelState(IRegionStorage regionStorage, IStaticDataService staticDataService,
            CurrenciesModel currenciesModel,
            ISceneLoader sceneLoader, ITileClickHandler tileClickHandler, ITileViewFactory tileViewFactory,
            ITilemapController tilemapController,
            ITileSpawner tileSpawner, ITileStorage tileStorage, ITransportSpawner transportSpawner,
            IPipelineSpawner pipelineSpawner, OrderGenerateService orderGenerateService,
            OrderTutorialGenerateService orderTutorialGenerateService, IQuestModel questModel,
            ITransportOrderSpawner transportOrderSpawner, OfflineProgressService offlineProgressService,
            CameraZoomController cameraZoom, PromoCodeModel promoCodeModel, WorkersService workersService,  ISettingModel settingModel,  AudioService audioService
            , IDailyEntryModel dailyEntryModel, IShopModel shopModel)
        {
            _regionStorage = regionStorage;
            _staticDataService = staticDataService;
            _currenciesModel = currenciesModel;
            _sceneLoader = sceneLoader;
            _tileClickHandler = tileClickHandler;
            _tileViewFactory = tileViewFactory;
            _tilemapController = tilemapController;
            _tileSpawner = tileSpawner;
            _tileStorage = tileStorage;
            _transportSpawner = transportSpawner;
            _pipelineSpawner = pipelineSpawner;
            _orderGenerateService = orderGenerateService;
            _orderTutorialGenerateService = orderTutorialGenerateService;
            _questModel = questModel;
            _transportOrderSpawner = transportOrderSpawner;
            _settingModel = settingModel;
            _audioService = audioService;
            _shopModel = shopModel;
            _offlineProgressService = offlineProgressService;
            _promoCodeModel = promoCodeModel;
            _workersService = workersService;
            _dailyEntryModel = dailyEntryModel;
            _cameraZoom = cameraZoom;
        }

        public void Enter(RegionModel payload)
        {
            _regionModel = payload;
            _currenciesModel.Initialize(_questModel);
            _staticDataService.LoadRegionConfig(payload.RegionType);
            _regionStorage.Initialize(payload.RegionProgress.Storage, _staticDataService);
            _orderGenerateService.Initialize();
            _orderTutorialGenerateService.Initialize();
            _sceneLoader.Load(Scene, OnLoaded);
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void OnLoaded()
        {
            _tileStorage.Load(_regionModel.RegionType);
            _tileClickHandler.Initialize();
            _tileViewFactory.Initialize();
            _tilemapController.Initialize();
            _offlineProgressService.Initialization();
            SpawnBuildings();
            _transportSpawner.Initialize();
            _pipelineSpawner.Initialize();
            _questModel.StartSession();
            _settingModel.Initialize();
            _audioService.Initialize(_settingModel,_staticDataService);
            _shopModel.Initialize();
            _offlineProgressService.InitializeProcessingProgress();
            _dailyEntryModel.Initializer();
            _promoCodeModel.Initialize();
            _workersService.Initialize();
            _cameraZoom.Initialize();
            _transportOrderSpawner.Initialize();

            _stateMachine.Enter<GameState>();
        }

        public void Exit()
        {
        }

        private void SpawnBuildings()
        {
            foreach (var buildingData in _tileStorage.TileMapData.BuildingsData)
                _tileSpawner.Spawn(buildingData);
        }
    }
}