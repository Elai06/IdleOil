using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.MoneyBox
{
    [CreateAssetMenu(fileName = "MoneyBoxConfig", menuName = "Configs/MoneyBoxConfig", order = 0)]
    public class MoneyBoxConfig : ScriptableObject
    {
        public List<MoneyBoxData> MoneyBoxesData;
    }
}
