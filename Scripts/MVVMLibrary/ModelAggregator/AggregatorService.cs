using System;
using System.Collections.Generic;
using Utils;
using Utils.ZenjectInstantiateUtil;

namespace MVVMLibrary.ModelAggregator
{
    public class AggregatorService : IAggregatorService
    {
        private readonly IInstantiateSpawner _instantiateSpawner;
        
        private Dictionary<Type, IAggregator> _cachedAggregators;

        public AggregatorService(IInstantiateSpawner instantiateSpawner)
        {
            _instantiateSpawner = instantiateSpawner;
            _cachedAggregators = new Dictionary<Type, IAggregator>();
        }

        public T Aggregate<T>() where T : class, IAggregator
        {
            var type = typeof(T);
            if (!_cachedAggregators.ContainsKey(type))
            {
                var aggregator = _instantiateSpawner.Instantiate<T>();
                _cachedAggregators.Add(type, aggregator);
                aggregator.Aggregate();
                return aggregator;
            }

            var cachedAggregator = _cachedAggregators[type];
            cachedAggregator.Aggregate();
            return cachedAggregator as T;
        }

        public T Aggregate<T, TPayload>(TPayload payload) where T : class, IAggregator<TPayload>
        {
            var type = typeof(T);
            if (!_cachedAggregators.ContainsKey(type))
            {
                var aggregator = _instantiateSpawner.Instantiate<T>();
                _cachedAggregators.Add(type, aggregator);
                aggregator.Aggregate(payload);
                return aggregator;
            }

            if (_cachedAggregators[type] is not IAggregator<TPayload> cachedAggregator)
            {
                this.LogError($"Invalid type: {type} for aggregation");
                return null;
            }

            cachedAggregator.Aggregate(payload);
            return cachedAggregator as T;
        }

        public T GetCachedAggregator<T>() where T : class, IAggregator
        {
            var type = typeof(T);
            if (!_cachedAggregators.ContainsKey(type))
            {
                this.LogError($"Aggregator of type: {type} is not cached");
            }
            var cachedAggregator = _cachedAggregators[type];
            return cachedAggregator as T;
        }
    }
}