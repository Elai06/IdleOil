using System;
using Gameplay.Store;
using UnityEngine.AddressableAssets;

namespace Gameplay.Shop
{
    [Serializable]
    public class HardProductData
    {
        public ProductType ProductType;
        public AssetReferenceSprite Sprite;
        public string Name;
        public Product Product;
        public int Cost;
    }
}