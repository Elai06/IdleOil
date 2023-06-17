using Gameplay.Services.Timer;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class
        ProcessingViewModelFactory : IViewModelFactory<ProcessingViewModel, ProcessingView, ProcessingWorkspaceModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly TimerService _timerService;

        public ProcessingViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider, TimerService timerService)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _timerService = timerService;
        }

        public ProcessingViewModel Create(ProcessingWorkspaceModel model, ProcessingView view) =>
            new(model, view, _assetProvider, _staticDataService, _timerService);
    }
}