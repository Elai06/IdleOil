using System;
using System.Collections.Generic;
using Gameplay.Store;
using UnityEngine.AddressableAssets;

namespace Gameplay.Shop
{
    [Serializable]
    public class PurchaseProductData 
    {
        public ProductType ProductType;
        public AssetReferenceSprite Sprite;
        public List<Product> Products = new();
        public int Cost;
        public string Name;
    }
}