using Infrastructure.StaticData;
using UnityEngine;

namespace Gameplay.Orders
{
    [CreateAssetMenu(fileName = "OrderGenerateConfig", menuName = "Configs/OrderConfig", order = 0)]
    public class OrderConfig : ScriptableObject
    { 
        public OrderGenerateConfigData Data;
    }
}