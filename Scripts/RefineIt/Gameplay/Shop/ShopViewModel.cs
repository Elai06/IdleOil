using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;

namespace Gameplay.Shop
{
    public class ShopViewModel : ViewModelBase<IShopModel, ShopView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public ShopViewModel(IShopModel model, ShopView view, IAssetProvider assetProvider,
            IStaticDataService staticDataService) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public override Task Show()
        {
            InitializeSetProducts();
            InitializeWorkerProducts();
            InitializeSoftProducts();
            InitializeHardProducts();

            return Task.CompletedTask;
        }

        private async void InitializeSetProducts()
        {
            View.SetProductSubViewContainer.CleanUp();
            await CreatePurchaseSubViews(Model.SetProducts, View.SetProductSubViewContainer);
        }

        private async void InitializeHardProducts()
        {
            View.HardProductSubViewContainer.CleanUp();
            await CreatePurchaseSubViews(Model.HardProducts, View.HardProductSubViewContainer);
        }

        private async void InitializeSoftProducts()
        {
            View.SoftProductSubViewContainer.CleanUp();
            await CreateCurrencySubViews(Model.SoftProducts, View.SoftProductSubViewContainer);
        }

        private async void InitializeWorkerProducts()
        {
            View.WorkerCurrencySubViewContainer.CleanUp();
            await CreateCurrencySubViews(Model.WorkerProducts, View.WorkerCurrencySubViewContainer);
        }

        private async Task CreateCurrencySubViews(List<CurrencyProductModel> purchaseProductModel,
            SubViewContainer<CurrencyProductSubView, CurrencyViewData> subViewContainer)
        {
            for (int i = 0; i < purchaseProductModel.Count; i++)
            {
                var model = purchaseProductModel[i];

                string id = model.Data.ProductType.ToString() + i;

                var viewData = new CurrencyViewData()
                {
                    PurchaseSprite = await _assetProvider.LoadSprite(model.Data.Sprite),
                    CostSprite = await _assetProvider.LoadSprite(_staticDataService.GetCurrencyData
                        (model.Data.CostProduct.CurrencyType).Sprite),
                    PurchaseProductData = model.Data
                };
                
                subViewContainer.Add(id, viewData);

                subViewContainer.SubViews[id].Click += model.Click;
            }
        }

        private async Task CreatePurchaseSubViews(List<PurchaseProductModel> purchaseProductModel,
            SubViewContainer<PurchaseSubView, PurchaseViewData> subViewContainer)
        {
            for (int i = 0; i < purchaseProductModel.Count; i++)
            {
                var data = purchaseProductModel[i];
                var model = purchaseProductModel[i];

                string id = data.Data.ProductType.ToString() + i;

                var viewData = new PurchaseViewData
                {
                    PurchaseSprite = await _assetProvider.LoadSprite(data.Data.Sprite),
                    PurchaseProductData = data.Data
                };

                subViewContainer.Add(id, viewData);
                
                subViewContainer.SubViews[id].Click += model.Click;
            }
        }
    }
}