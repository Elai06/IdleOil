using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Currencies;
using Infrastructure.StaticData;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Purchasing
{
    [UsedImplicitly]
    public class GenericPurchasingService : IPurchasingService, IInitializable, IDisposable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IInAppPurchasingService _inAppPurchasingService;
        private readonly CurrenciesModel _currenciesModel;
        
        private GenericPurchasingService(
            IStaticDataService staticDataService, 
            IInAppPurchasingService inAppPurchasingService,
            CurrenciesModel currenciesModel)
        {
            _staticDataService = staticDataService;
            _inAppPurchasingService = inAppPurchasingService;
            _currenciesModel = currenciesModel;
        }

        public void Initialize()
        {
            _inAppPurchasingService.OnPurchasingInitialized += PurchasingInitialized;
            _inAppPurchasingService.OnPurchased += Purchased;
        }
        
        public void Dispose()
        {
            _inAppPurchasingService.OnPurchasingInitialized -= PurchasingInitialized;
            _inAppPurchasingService.OnPurchased -= Purchased;
        }
        
        private void PurchasingInitialized(object _, bool isInitialized)
        {
            OnPurchasingInitialized?.Invoke(this, isInitialized);
        }
        
        private void Purchased(object _, PurchaseResult result)
        {
            OnPurchased?.Invoke(this, result);
        }

        #region IPurchaseService

        public UniTask<bool> InitializePurchasing()
        {
            return _inAppPurchasingService.InitializePurchasing();
        }

        public bool IsPurchasingInitialized()
        {
            return _inAppPurchasingService.IsPurchasingInitialized();
        }

        public event EventHandler<bool> OnPurchasingInitialized;

        public UniTask<IEnumerable<Purchase>> GetPurchases()
        {
            return _inAppPurchasingService.GetPurchases();
        }

        public UniTask<bool> Purchase(string title)
        {
            return Purchase(_staticDataService.GetPurchase(title));
        }

        public async UniTask<bool> Purchase(PurchaseStaticData purchase)
        {
            // perform in-app logic
            if (purchase.IsInApp)
            {
                var isSuccess = await _inAppPurchasingService.Purchase(purchase);
                if (!isSuccess)
                {
                    throw new PurchaseUnsuccessfulException();
                }
            }
            else
            {
                // check if user has enough currency
                if (!_currenciesModel.Has(purchase.Price))
                {
                    throw new NotEnoughCurrencyException();
                }
            
                // remove user currency
                _currenciesModel.Consume(purchase.Price);
            }
            
            // add user currency
            _currenciesModel.Add(purchase.Currencies);
            return true;
        }

        public event EventHandler<PurchaseResult> OnPurchased;
        
        #endregion
    }
}