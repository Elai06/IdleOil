using Gameplay.Region;
using Zenject;

namespace Gameplay.MoneyBox
{
    public class GetCurrenciesButtonMoneyBox : ButtonBaseSFX
    {
        private MoneyBoxModel _moneyBoxModel;

        [Inject]
        public void Construct(MoneyBoxModel moneyBoxModel)
        {
            _moneyBoxModel = moneyBoxModel;
        }

        public override void OnClick()
        {
            base.OnClick();
        
            _moneyBoxModel.GetHardCurrencies();
        }
    }
}