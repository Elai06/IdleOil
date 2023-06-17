using MVVMLibrary.Enums;

namespace MVVMLibrary.Base.View
{
    public interface IView
    {
        public void SetViewModel<T>(T viewModel) where T : ViewModel.ViewModel;
        public Layer EUiLayer { get; }
        public void Show();
        public void Hide();
    }
}