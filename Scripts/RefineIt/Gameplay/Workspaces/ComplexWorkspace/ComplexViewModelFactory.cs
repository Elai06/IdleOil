using Gameplay.Region.Storage;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class ComplexViewModelFactory : IViewModelFactory<ComplexViewModel, ComplexView, ComplexWorkspaceModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly IRegionStorage _regionStorage;

        public ComplexViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider, IRegionStorage regionStorage)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _regionStorage = regionStorage;
        }

        public ComplexViewModel Create(ComplexWorkspaceModel model, ComplexView view) =>
            new(model, view, _assetProvider, _staticDataService, _regionStorage);
    }
}