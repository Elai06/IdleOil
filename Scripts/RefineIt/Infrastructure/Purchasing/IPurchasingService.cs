using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing;

namespace Infrastructure.Purchasing
{
    public interface IPurchasingService
    {
        UniTask<bool> InitializePurchasing();
        bool IsPurchasingInitialized();
        event EventHandler<bool> OnPurchasingInitialized;

        UniTask<IEnumerable<Purchase>> GetPurchases();

        UniTask<bool> Purchase(string title);
        UniTask<bool> Purchase(PurchaseStaticData purchase);
        event EventHandler<PurchaseResult> OnPurchased;
    }

    public class PurchaseResult
    {
        public readonly bool IsSuccess;
        public readonly PurchaseStaticData Data;

        public PurchaseResult(bool isSuccess, PurchaseStaticData data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
    }

    public class Purchase
    {
        public readonly Product Product;
        public readonly PurchaseStaticData Data;
        
        public Purchase(Product product, PurchaseStaticData data)
        {
            Product = product;
            Data = data;
        }
    }
}