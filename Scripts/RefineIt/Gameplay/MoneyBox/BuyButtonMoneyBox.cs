using Gameplay.Region;
using Zenject;

namespace Gameplay.MoneyBox
{
    public class BuyButtonMoneyBox : ButtonBaseSFX
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

            if (_moneyBoxModel.ProgressData.IsBuy)
            {
                _moneyBoxModel.Upgrade();
            }
            else
            {
                _moneyBoxModel.Buy();
            }
        }
    }
}