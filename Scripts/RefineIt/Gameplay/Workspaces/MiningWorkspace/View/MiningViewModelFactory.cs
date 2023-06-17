using Gameplay.Region.Storage;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Workspaces.MiningWorkspace.View
{
    public class MiningViewModelFactory : IViewModelFactory<MiningViewModel, MiningView, MiningWorkSpaceModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly IRegionStorage _regionStorage;

        public MiningViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider, IRegionStorage regionStorage)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _regionStorage = regionStorage;
        }

        public MiningViewModel Create(MiningWorkSpaceModel model, MiningView view) =>
            new(model, view, _assetProvider, _staticDataService, _regionStorage);
    }
}