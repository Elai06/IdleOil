using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Quests
{
    [CreateAssetMenu(fileName = "DailyQuest", menuName = "Data/DailyQuest")]
    public class QuestStaticData : ScriptableObject
    {
        public List<QuestComponent> QuestComponents;
    }
}