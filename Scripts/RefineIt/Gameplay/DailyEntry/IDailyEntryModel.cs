using Gameplay.Currencies;

namespace Gameplay.DailyEntry
{
    public interface IDailyEntryModel
    {
        void Initializer();
        DailyEntryComponentData GetComponentData(DailyEntryType day);
        DailyEntryProgress GetProgress(DailyEntryType day);
        DailyEntryType GetCurDailyEntryType();
        void TakeReward(DailyEntryType day , CurrencyType currencyType, int reward);
    }
}