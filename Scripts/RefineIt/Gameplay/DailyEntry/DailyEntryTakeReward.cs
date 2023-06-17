using Gameplay.Currencies;
using Gameplay.Region;
using System;

namespace Gameplay.DailyEntry
{
    public class DailyEntryTakeReward : ButtonBaseSFX
    {
        public event Action<DailyEntryType,CurrencyType,int> Click;
        private DailyEntryType _day;
        private CurrencyType _currencyType;
        private int _amount;

        public void Initialize(DailyEntryType day,CurrencyType currencyType, int amount)
        {
            _day = day;
            _currencyType = currencyType;
            _amount = amount;
        }

        public override void OnClick()
        {
            base.OnClick();
            Click?.Invoke(_day,_currencyType,_amount);
            gameObject.SetActive(false);
        }
    }
}