namespace MVVMLibrary.ModelAggregator
{
    public abstract class AbstractAggregator<T> : IAggregator<T>
    {
        public abstract void Aggregate();

        public virtual void Aggregate(T payload)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public abstract class AbstractAggregator : IAggregator
    {
        public abstract void Aggregate();
    }
}