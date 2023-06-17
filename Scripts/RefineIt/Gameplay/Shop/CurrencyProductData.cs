using System;
using Gameplay.Store;
using UnityEngine.AddressableAssets;

namespace Gameplay.Shop
{
    [Serializable]
    public class CurrencyProductData
    {
        public ProductType ProductType;
        public string Name;
        public AssetReferenceSprite Sprite;
        public Product PurchaseProduct;
        public Product CostProduct;
    }
}