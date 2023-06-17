using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "HardProductData", menuName = "Configs/HardProductData", order = 0)]
    public class ShopHardProductConfig : ScriptableObject
    {
        public List<PurchaseProductData> HardProducts;
    }
}