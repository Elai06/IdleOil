using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Configs/ShopConfig", order = 0)]
    public class ShopConfigData : ScriptableObject
    {
        public ShopCurrencyDataData SoftProducts;
        public ShopCurrencyDataData WorkerProducts;
        public ShopHardProductConfig HardProducts;
        public ShopSetProductConfig SetProducts;
    }
}