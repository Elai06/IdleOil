using System.Threading.Tasks;
using Infrastructure.Windows.MVVM;

namespace Gameplay.PromoCode
{
    public class PromoCodeViewModel : ViewModelBase<PromoCodeModel, PromoCodeView>
    {
        public PromoCodeViewModel(PromoCodeModel model, PromoCodeView view) : base(model, view)
        {
        }

        public override Task Show()
        {
            return null;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            View.EnterPromoCodeButton += EnterPromoCode;
            Model.PromoCodeStatus += OnPromoCodeStatus;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            View.EnterPromoCodeButton -= EnterPromoCode;
            Model.PromoCodeStatus -= OnPromoCodeStatus;
        }

        private void OnPromoCodeStatus(PromoCodeStatusType status)
        {
            View.ShowPromoCodeStatus(status);
        }

        private void EnterPromoCode(string key)
        {
            Model.EnterPromoCode(key);
        }
    }
}