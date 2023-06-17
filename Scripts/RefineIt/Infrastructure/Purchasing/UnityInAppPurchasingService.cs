using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Cysharp.Threading.Tasks;
using Infrastructure.StaticData;
using JetBrains.Annotations;
using UnityEngine.Purchasing;
using Utils;
using Zenject;

namespace Infrastructure.Purchasing
{
    [UsedImplicitly]
    public class UnityInAppPurchasingService : IInitializable, IDisposable, IInAppPurchasingService, IStoreListener
    {
        private readonly IStaticDataService _staticDataService;
        private IStoreController _storeController;
        // ReSharper disable once InconsistentNaming
        private UniTaskCompletionSource<bool> _initializeTaskCS;
        // ReSharper disable once InconsistentNaming
        private UniTaskCompletionSource<bool> _purchaseTaskCS;
        
        private UnityInAppPurchasingService(
            IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Initialize()
        {
            OnPurchasingInitialized += PurchasingInitialized;
            OnPurchased += Purchased;
            InitializePurchasing();
        }
        
        public void Dispose()
        {
            OnPurchasingInitialized -= PurchasingInitialized;
            OnPurchased -= Purchased;
        }

        private async UniTask EnsureInitialized()
        {
            if (IsPurchasingInitialized())
            {
                return;
            }
            
            await InitializePurchasing();

            if (!IsPurchasingInitialized())
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASING_NOT_INITIALIZED);
            }
        }
        
        private void PurchasingInitialized(object _, bool isInitialized)
        {
            if (_initializeTaskCS == null)
            {
                return;
            }

            _initializeTaskCS.TrySetResult(isInitialized);
            _initializeTaskCS = null;
        }
        
        private void Purchased(object _, PurchaseResult result)
        {
            if (_purchaseTaskCS == null)
            {
                return;
            }

            _purchaseTaskCS.TrySetResult(result.IsSuccess);
            _purchaseTaskCS = null;
        }

        #region IInAppPurchaseService
        
        public UniTask<bool> InitializePurchasing()
        {
            if (_initializeTaskCS != null)
            {
                // NOTE: potential exception if Purchase called before Initialization is complete
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASING_INITIALIZATION_IN_PROGRESS);
                return _initializeTaskCS.Task;
            }
            
            var inApps = _staticDataService.GetInAppPurchases().ToArray();
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var inApp in inApps)
            {
                builder.AddProduct(inApp.Title, ProductType.Consumable);
            }
            
            _initializeTaskCS = new UniTaskCompletionSource<bool>();
            UnityPurchasing.Initialize(this, builder);
            return _initializeTaskCS.Task;
        }

        public bool IsPurchasingInitialized()
        {
            return _storeController != null;
        }

        public event EventHandler<bool> OnPurchasingInitialized;

        public async UniTask<IEnumerable<Purchase>> GetPurchases()
        {
            await EnsureInitialized();
            
            return from product in _storeController.products.all
                select new Purchase(product, _staticDataService.GetPurchase(product.definition.id));
        }

        public UniTask<bool> Purchase(string title)
        {
            return Purchase(_staticDataService.GetPurchase(title));
        }

        public async UniTask<bool> Purchase(PurchaseStaticData purchase)
        {
            await EnsureInitialized();
            
            if (!purchase.IsInApp)
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_NOT_IN_APP);
            }

            // perform in-app logic
            if (_purchaseTaskCS != null)
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_IN_PROGRESS);
            }
            
            _purchaseTaskCS = new UniTaskCompletionSource<bool>();
            _storeController.InitiatePurchase(purchase.Title);
            return await _purchaseTaskCS.Task;
        }

        public event EventHandler<PurchaseResult> OnPurchased;

        #endregion
        
        #region IStoreListener

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.Log("In-App Purchasing successfully initialized");
            _storeController = controller;
            
            OnPurchasingInitialized?.Invoke(this, true);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            this.LogError($"error: {error}");
            
            OnPurchasingInitialized?.Invoke(this, false);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            this.LogError($"error: {error} message: {message}");
            
            OnPurchasingInitialized?.Invoke(this, false);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var result = new PurchaseResult(true, _staticDataService.GetPurchase(product.definition.id));
            OnPurchased?.Invoke(this, result);
            
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            this.LogError($"product: {product.definition.id} reason: {failureReason}");

            throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_UNSUCCESSFUL_WITH_REASON);
            
            var result = new PurchaseResult(false, _staticDataService.GetPurchase(product.definition.id));
            OnPurchased?.Invoke(this, result);
        }

        #endregion
    }
}