using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "ShopCurrencyDataData", menuName = "Configs/ShopCurrencyDataData", order = 0)]
    public class ShopCurrencyDataData : ScriptableObject
    {
        public List<CurrencyProductData> CurrencyProducts;
    }
}