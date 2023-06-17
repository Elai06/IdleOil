using Infrastructure.Windows.MVVM;

namespace Gameplay.PromoCode
{
    public class PromoCodeViewModeFactory : IViewModelFactory<PromoCodeViewModel, PromoCodeView, PromoCodeModel>
    {
        public PromoCodeViewModel Create(PromoCodeModel model, PromoCodeView view) => new(model, view);
    }
}