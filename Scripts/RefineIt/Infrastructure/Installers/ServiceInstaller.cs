using Gameplay.CameraProject;
using Gameplay.Currencies;
using Gameplay.DailyEntry;
using Gameplay.DailyEntry.UI;
using Gameplay.MobileNotification;
using Gameplay.MoneyBox;
using Gameplay.OfflineProgressService;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Personnel;
using Gameplay.PromoCode;
using Gameplay.Quests;
using Gameplay.Quests.UI;
using Gameplay.Region;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Gameplay.Settings;
using Gameplay.Settings.UI;
using Gameplay.Shop;
using Gameplay.Store;
using Gameplay.Tilemaps.Factories;
using Gameplay.Tilemaps.Services;
using Gameplay.Workers;
using Gameplay.Workspaces;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace.View;
using Gameplay.Workspaces.ProcessingWorkspace;
using Gameplay.Workspaces.Workers.Pipeline;
using Gameplay.Workspaces.Workers.Transport;
using Gameplay.Workspaces.Workers.Wagon;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.Purchasing;
using Infrastructure.SaveLoads;
using Infrastructure.SceneManagement;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using Infrastructure.YandexAds;
using MVVMLibrary.ModelAggregator;
using UnityEngine;
using Utils.Pools;
using Utils.Services;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private UnityPoolFactory _unityPoolFactory;
        [SerializeField] private UnityMainThreadDispatcherService _dispatcher;
        [SerializeField] private TimerService _timerService;
        [SerializeField] private CoroutineService coroutineService;
        [SerializeField] private LayersContainer _layersContainer;
        [SerializeField] private TilemapClickHandler _tilemapClickHandler;
        [SerializeField] private AudioService _audioService;
        [SerializeField] private OfflineProgressService _offlineProgressService;
        [SerializeField] private YandexAdsService yandexAdsService;
        [SerializeField] private CameraZoomController cameraZoomController;

        public override void InstallBindings()
        {
            BindInfrastructureServices();
            BindModels();

            Container
                .Bind<IDispatcherService>()
                .To<UnityMainThreadDispatcherService>()
                .FromInstance(_dispatcher)
                .AsSingle();

            Container
                .Bind<IAggregatorService>()
                .To<AggregatorService>()
                .AsSingle();

            Container
                .Bind<IPoolFactory>()
                .To<UnityPoolFactory>()
                .FromInstance(_unityPoolFactory)
                .AsSingle();

            Container
                .Bind<PoolSystem>()
                .AsSingle();

            Container
                .Bind<TimerService>()
                .FromInstance(_timerService)
                .AsSingle();

            Container
                .Bind<AudioService>()
                .FromInstance(_audioService);

            Container
                .Bind<OfflineProgressService>()
                .FromInstance(_offlineProgressService)
                .AsSingle();

            Container.Bind<IAdsService>()
                .To<YandexAdsService>()
                .FromInstance(yandexAdsService)
                .AsSingle();

            Container.Bind<CameraZoomController>()
                .FromInstance(cameraZoomController);
            
            Container.Bind<WorkersService>()
                .AsSingle();
        }

        private void BindInfrastructureServices()
        {
            BindGameStates();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IPurchasingService>().To<GenericPurchasingService>().AsSingle();
            Container.Bind<IInAppPurchasingService>().To<UnityInAppPurchasingService>().AsSingle();
            Container.Bind<IProgressService>().To<PlayerProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<ICoroutineService>().FromInstance(coroutineService).AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IWorkspaceService>().To<WorkspaceService>().AsSingle();
            Container.Bind<IWorkspaceFactory>().To<WorkspaceFactory>().AsSingle();
            Container.Bind<IUnityViewFactory>().To<UnityViewFactory>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IRegionStorage>().To<RegionStorage>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
            Container.Bind<IRegionFactory>().To<RegionFactory>().AsSingle();
            Container.Bind<LayersContainer>().FromInstance(_layersContainer).AsSingle();
            Container.Bind<ITileClickHandler>().FromInstance(_tilemapClickHandler).AsSingle();
            Container.Bind<ITileStorage>().To<TileStorage>().AsSingle();
            Container.Bind<ITileSpawner>().To<TileSpawner>().AsSingle();
            Container.Bind<IBuildingFactory>().To<BuildingFactory>().AsSingle();
            Container.Bind<ITileViewFactory>().To<TileViewFactory>().AsSingle();
            Container.Bind<ITilemapController>().To<TilemapController>().AsSingle();
            Container.Bind<ITransportSpawner>().To<TransportSpawner>().AsSingle();
            Container.Bind<ITransportFactory>().To<TransportFactory>().AsSingle();
            Container.Bind<ITransportOrderFactory>().To<TransportOrderFactory>().AsSingle();
            Container.Bind<ITransportOrderSpawner>().To<TransportOrderSpawner>().AsSingle();
            Container.Bind<IBuildingService>().To<BuildingService>().AsSingle();
            Container.Bind<IPipelineFactory>().To<PipelineFactory>().AsSingle();
            Container.Bind<IPipelineSpawner>().To<PipelineSpawner>().AsSingle();
            Container.Bind<IQuestModel>().To<QuestsModel>().AsSingle();
            Container.Bind<ISettingModel>().To<SettingModel>().AsSingle();
            Container.Bind<IMobileNotification>().To<MobileNotificationService>().AsSingle();
            Container.Bind<IShopModel>().To<ShopModel>().AsSingle();
            Container.Bind<IDailyEntryModel>().To<DailyEntryModel>().AsSingle();
        }

        private void BindModels()
        {
            Container.BindInterfacesAndSelfTo<CurrenciesModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<OrdersModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoneyBoxModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<OrderGenerateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<OrderTutorialGenerateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrencyViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ComplexViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiningViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<OrdersViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoneyBoxViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorkersViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<StorageViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProcessingViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<DailyQuestViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeeklyQuestViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SettingViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PromoCodeModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PromoCodeViewModeFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopViewModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<DailyEntryViewModelFactory>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.Bind<IStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<ExitState>().AsSingle();
            Container.Bind<GameState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<BootstrapState>().AsSingle();
        }
    }
}