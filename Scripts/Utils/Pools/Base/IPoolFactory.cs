using UnityEngine;

namespace Utils.Pools
{
    public interface IPoolFactory
    {
        IPool CreatePool<T>(T prefab) where T : PoolObject;
    }
}