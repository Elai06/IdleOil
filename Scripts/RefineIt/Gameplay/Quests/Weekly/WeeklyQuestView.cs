using Gameplay.Region.Storage;
using UnityEngine;

namespace Gameplay.Quests.UI
{
    public class WeeklyQuestView : MonoBehaviour
    {
        [field:SerializeField] public WeeklyQuestContainer WeeklyQuestContainer{ get; private set; }
    }
}