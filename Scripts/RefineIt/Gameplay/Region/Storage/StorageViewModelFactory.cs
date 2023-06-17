using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Region.Storage
{
    public class StorageViewModelFactory : IViewModelFactory<RegionViewModel, RegionView, IRegionStorage>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public StorageViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public RegionViewModel Create(IRegionStorage model, RegionView view) => 
            new RegionViewModel(model, view, _assetProvider, _staticDataService);
    }
}