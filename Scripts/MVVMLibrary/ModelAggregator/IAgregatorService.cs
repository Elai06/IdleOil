namespace MVVMLibrary.ModelAggregator
{
    public interface IAggregatorService
    {
        T Aggregate<T>() where T : class, IAggregator;
        T Aggregate<T, TPayload>(TPayload payload) where T : class, IAggregator<TPayload>;
        T GetCachedAggregator<T>() where T : class, IAggregator;
    }
}