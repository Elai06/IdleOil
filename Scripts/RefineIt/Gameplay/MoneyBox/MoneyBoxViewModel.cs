using System.Threading.Tasks;
using Infrastructure.Windows.MVVM;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxViewModel : ViewModelBase<MoneyBoxModel, MoneyBoxView>
    {
        public MoneyBoxViewModel(MoneyBoxModel model, MoneyBoxView view) : base(model, view)
        {
        }

        public override Task Show()
        {
            UpdateView();
           
           return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Model.OnChange += UpdateView;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            
            Model.OnChange -= UpdateView;
        }

        private void UpdateView()
        {
            View.SetLevel(Model.ProgressData.Level + 1);
            View.SetQuantity(Model.ProgressData.Amount);
            View.SetData(Model.ProgressData.IsBuy, Model.IsMaxLevel);
            View.SetCapacity(Model.Config.Capacities, Model.NextLevelConfig.Capacities);
            View.SetSlider(Model.ProgressData.Amount, Model.Config.Capacities);
            View.SetButtonsStatus(Model.IsFillMoneyBox, Model.ProgressData.IsBuy);
        }
    }
}