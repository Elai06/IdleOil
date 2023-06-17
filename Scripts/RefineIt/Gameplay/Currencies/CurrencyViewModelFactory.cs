using Infrastructure.AssetManagement;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Currencies
{
    public class CurrencyViewModelFactory : IViewModelFactory<CurrencyViewModel, CurrencyView, CurrencyModel>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IWindowService _windowService;

        public CurrencyViewModelFactory(IAssetProvider assetProvider, IWindowService windowService)
        {
            _assetProvider = assetProvider;
            _windowService = windowService;
        }

        public CurrencyViewModel Create(CurrencyModel model, CurrencyView view) =>
            new CurrencyViewModel(model, view, _assetProvider, _windowService);
    }
}