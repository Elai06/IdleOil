using Gameplay.Currencies;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.DailyEntry.UI
{
    public class DailyEntrySubData
    {
        public Sprite Sprite;
        public string Description;
        public string Reward;
        public bool IsTake;
        public bool IsShowRewardTake;
        public DailyEntryType Day;
        public CurrencyType Currency;

        public DailyEntrySubData(Sprite sprite, string description, string reward, bool isTake, bool isShowRewardTake,
        DailyEntryType day, CurrencyType currencyType)
        {
            Sprite = sprite;
            Description = description;
            Reward = reward;
            IsTake = isTake;
            IsShowRewardTake = isShowRewardTake;
            Day = day;
            Currency = currencyType;
        }

        public DailyEntrySubData(bool isTake, bool isShowRewardTake)
        {
            IsTake = isTake;
            IsShowRewardTake = isShowRewardTake;
        }
    }
}