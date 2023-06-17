using System;
using Gameplay.Store;

namespace Gameplay.Shop
{
    public class CurrencyProductModel
    {
        public event Action<CurrencyProductModel> BuyClick;

        public CurrencyProductModel(ShopCurrencyProductProgress shopCurrencyProductProgress, CurrencyProductData data)
        {
            ShopCurrencyProductProgress = shopCurrencyProductProgress;
            Data = data;
        }

        public ShopCurrencyProductProgress ShopCurrencyProductProgress { get; private set; }
        
        public CurrencyProductData Data { get; private set; }
        
        public void Click()
        {
            BuyClick?.Invoke(this);
        }
    }
}