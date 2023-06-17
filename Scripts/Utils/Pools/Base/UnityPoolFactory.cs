using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Utils.Pools
{
    public class UnityPoolFactory : MonoBehaviour, IPoolFactory
    {
        [SerializeField] private UnityPoolConfig _poolConfig;
        
        private IGameObjectDestroyer _destroyer;
        private IGameObjectFactory _gameObjectFactory;

        [Inject]
        private void Construct(IGameObjectFactory gameObjectFactory, IGameObjectDestroyer destroyer)
        {
            _gameObjectFactory = gameObjectFactory;
            _destroyer = destroyer;
        }

        public IPool CreatePool<T>(T prefab) where T : PoolObject
        {
            var pool = new UnityPool(_gameObjectFactory, _destroyer);
            pool.SetPoolObject(prefab, gameObject.transform, _poolConfig.CollectionCheck, 
                _poolConfig.InitialCapacity, _poolConfig.MaxSize);
            
            return pool;
        }
    }

}