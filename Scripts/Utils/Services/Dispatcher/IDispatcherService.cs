namespace Utils.Services
{
    public interface IDispatcherService
    {
        void InvokeOnMainThread(System.Action action);
    }
}