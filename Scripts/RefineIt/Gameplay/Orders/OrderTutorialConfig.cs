using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Orders
{
    [CreateAssetMenu(fileName = "OrderTutorialConfig", menuName = "Configs/OrderTutorialConfig", order = 0)]
    public class OrderTutorialConfig : ScriptableObject
    {
        public List<OrderConfigData> OrderConfigsData;
    }
}