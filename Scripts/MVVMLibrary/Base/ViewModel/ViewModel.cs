namespace MVVMLibrary.Base.ViewModel
{
    public abstract class ViewModel
    {
        public abstract void AddSubscriptions();

        public abstract void SetData();

        public abstract void RemoveSubscriptions();
    }
}