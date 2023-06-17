using Infrastructure.Windows.MVVM;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxViewModelFactory : IViewModelFactory<MoneyBoxViewModel, MoneyBoxView, MoneyBoxModel>
    {
        public MoneyBoxViewModel Create(MoneyBoxModel model, MoneyBoxView view)
        {
            return new MoneyBoxViewModel(model, view);
        }
    }
}