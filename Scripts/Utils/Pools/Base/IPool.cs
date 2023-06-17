using UnityEngine;

namespace Utils.Pools
{
    public interface IPool
    {
        PoolObject Prefab { get; }

        public void SetPoolObject(PoolObject prefab, Transform parent, bool collectionCheck,
            int defaultCapacity, int maxSize = 10000);
        
        PoolObject GetFreeElement(Vector3 position);
        PoolObject GetFreeElement(Vector3 position, Quaternion rotation);
        PoolObject GetFreeElement(Vector3 position, Quaternion rotation, Transform parent);
        void ReturnToPool(PoolObject poolObject);
    }

}
