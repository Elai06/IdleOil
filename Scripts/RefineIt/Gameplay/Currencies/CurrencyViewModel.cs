using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Currencies
{
    public class CurrencyViewModel : ViewModelBase<CurrencyModel, CurrencyView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IWindowService _windowService;

        public CurrencyViewModel(CurrencyModel model, CurrencyView view, IAssetProvider assetProvider, IWindowService windowService) :
            base(model, view)
        {
            _assetProvider = assetProvider;
            _windowService = windowService;
        }

        public override async Task Show()
        {
            View.ViewAmount(Model.Amount);
            View.ViewIcon(await _assetProvider.LoadSprite(Model.SpriteReference));
        }

        public override void Subscribe()
        {
            base.Subscribe();
            Model.AmountUpdated += OnUpdateAmount;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            Model.AmountUpdated -= OnUpdateAmount;
        }

        protected override async void OnClicked()
        {
            await _windowService.Open(WindowType.Shop);
        }

        private void OnUpdateAmount() => 
            View.ViewAmount(Model.Amount);
    }
}