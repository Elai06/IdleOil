using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Zenject;

namespace Gameplay.Shop
{
    public class ShopViewModelFactory : IViewModelFactory<ShopViewModel, ShopView, IShopModel>
    {
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;

        [Inject]
        public void Construct(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public ShopViewModel Create(IShopModel model, ShopView view) =>
            new(model, view, _assetProvider, _staticDataService);
    }
}