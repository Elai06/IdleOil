using System.Collections.Generic;

namespace Utils.Pools
{
    public class PoolSystem
    {
        private Dictionary<PoolObject, IPool> _pools;

        private IPoolFactory _poolFactory;

        public PoolSystem(IPoolFactory poolFactory)
        {
            _poolFactory = poolFactory;
            _pools = new Dictionary<PoolObject, IPool>();
        }

        public IPool GetPool<T>(T prefab) where T : PoolObject
        {
            if (_pools.ContainsKey(prefab))
            {
                return _pools[prefab];
            }
            
            var pool = _poolFactory.CreatePool(prefab);
            _pools.Add(prefab, pool);
            return pool;
        }

        public void ReturnObjectToPool(PoolObject poolObject)
        {
            if (_pools.TryGetValue(poolObject.PrefabReference, out var pool))
            {
                pool.ReturnToPool(poolObject);
            }
        }
    }
}