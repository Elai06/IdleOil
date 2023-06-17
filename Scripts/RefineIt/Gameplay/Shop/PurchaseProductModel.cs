using System;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Shop
{
    public class PurchaseProductModel
    {
        public event Action<PurchaseProductModel> BuyClick;
        
        public PurchaseProductModel(ShopPurchaseProductProgress shopCurrencyProductProgress, PurchaseProductData data)
        {
            ShopCurrencyProductProgress = shopCurrencyProductProgress;
            Data = data;
        }

        public ShopPurchaseProductProgress ShopCurrencyProductProgress { get; private set; }
        public PurchaseProductData Data { get; private set; }

        public void Click()
        {
            BuyClick?.Invoke(this);
        }
    }
}