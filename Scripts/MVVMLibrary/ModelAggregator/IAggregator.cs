namespace MVVMLibrary.ModelAggregator
{
    public interface IAggregator
    {
        void Aggregate();
    }

    public interface IAggregator<in T> : IAggregator
    {
        void Aggregate(T payload);
    }
}