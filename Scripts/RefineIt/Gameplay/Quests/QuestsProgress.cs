using System;
using Gameplay.Quests;

namespace Gameplay.Currencies
{
    [Serializable]
    public class QuestsProgress
    {
        public int QuestProgress;
        public QuestsGuid Guid;
        public bool IsTakeReward;
        
        public QuestsProgress(int questProgress, QuestsGuid guid, bool isTakeReward)
        {
            QuestProgress = questProgress;
            Guid = guid;
            IsTakeReward = isTakeReward;
        }
    }
}