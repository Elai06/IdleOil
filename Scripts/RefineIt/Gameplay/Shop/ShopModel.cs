using System.Collections.Generic;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Shop
{
    public class ShopModel : IShopModel
    {
        public List<CurrencyProductModel> SoftProducts { get; private set; } = new();
        public List<CurrencyProductModel> WorkerProducts { get; private set; } = new();
        public List<PurchaseProductModel> HardProducts { get; private set; } = new();
        public List<PurchaseProductModel> SetProducts { get; private set; } = new();

        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;

        public ShopModel(IStaticDataService staticDataService, IProgressService progressService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
        }

        public void Initialize()
        {
            CreateSoftProducts();
            CreateWorkerProducts();
            CreateHardProducts();
            CreateSetProducts();
        }

        private void CreateSoftProducts()
        {
            for (int i = 0; i < _staticDataService.ShopConfigData.SoftProducts.CurrencyProducts.Count; i++)
            {
                var data = _staticDataService.ShopConfigData.SoftProducts.CurrencyProducts[i];
                string id = data.ProductType.ToString() + i;

                SoftProducts.Add(CreateCurrencyProductModel(id, data));
            }
        }

        private void CreateWorkerProducts()
        {
            for (int i = 0; i < _staticDataService.ShopConfigData.WorkerProducts.CurrencyProducts.Count; i++)
            {
                var data = _staticDataService.ShopConfigData.WorkerProducts.CurrencyProducts[i];
                string id = data.ProductType.ToString() + i;

                WorkerProducts.Add(CreateCurrencyProductModel(id, data));
            }
        }
        
        private void CreateSetProducts()
        {
            for (int i = 0; i < _staticDataService.ShopConfigData.SetProducts.SetProducts.Count; i++)
            {
                var data = _staticDataService.ShopConfigData.SetProducts.SetProducts[i];
                string id = data.ProductType.ToString() + i;

                SetProducts.Add(CreatePurchaseProductModel(id, data));
            }
        }
        
        private void CreateHardProducts()
        {
            for (int i = 0; i < _staticDataService.ShopConfigData.HardProducts.HardProducts.Count; i++)
            {
                var data = _staticDataService.ShopConfigData.HardProducts.HardProducts[i];
                string id = data.ProductType.ToString() + i;

                HardProducts.Add(CreatePurchaseProductModel(id, data));
            }
        }

        private PurchaseProductModel CreatePurchaseProductModel(string id, PurchaseProductData data)
        {
            var progress = _progressService.PlayerProgress
                .GetOrCreatePurchaseProductProgress(id);
            
            var model = new PurchaseProductModel(progress, data);

            model.BuyClick += OnPurchaseBuyClick;

            return model;
        }

        private CurrencyProductModel CreateCurrencyProductModel(string id, CurrencyProductData data)
        {
            var progress = _progressService.PlayerProgress
                .GetOrCreateCurrencyProductProgress(id);

            var model = new CurrencyProductModel(progress, data);

            model.BuyClick += OnCurrencyBuyClick;
            
            return model;
        }

        private void OnCurrencyBuyClick(CurrencyProductModel data)
        {
            //ToDO: покупка
        }
        
        private void OnPurchaseBuyClick(PurchaseProductModel data)
        {
            //ToDO: покупка
        }
    }
}