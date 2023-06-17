using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Shop
{
    [Serializable]
    public class ProductViewData
    {
        public ProductType ProductType;
        public string Name;
        public Sprite ProductSprite;
        public List<ProductData> Purchases = new();
        public ProductData Cost = new ProductData();
    }
}