using System;

namespace Gameplay.DailyEntry
{
    [Serializable]
    public class DailyEntryProgress
    {
        public DailyEntryType Day;
        public int CircleTake;
        public bool IsVisableReward;
        public bool IsTake;

        public DailyEntryProgress(DailyEntryType day, int circleTake,bool isVisableReward, bool isTake)
        {
            Day = day;
            CircleTake = circleTake;
            IsVisableReward = isVisableReward;
            IsTake = isTake;
        }
    }
}