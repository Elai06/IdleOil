using UnityEngine;
using UnityEngine.Pool;
using Utils.ZenjectInstantiateUtil;

namespace Utils.Pools
{
    public class UnityPool : IPool
    {
        private ObjectPool<PoolObject> _pool;
        
        private IGameObjectFactory _gameObjectFactory;
        private IGameObjectDestroyer _destroyer;
        private Transform _parentForPoolObjects;

        public PoolObject Prefab { get; private set; }
        
        public UnityPool (IGameObjectFactory gameObjectFactory, IGameObjectDestroyer destroyer)
        {
            _gameObjectFactory = gameObjectFactory;
            _destroyer = destroyer;
        }
        
        public void SetPoolObject(PoolObject prefab, Transform parent, bool collectionCheck,
            int defaultCapacity, int maxSize = 10000)
        {
            _parentForPoolObjects = parent;
            Prefab = prefab;
            _pool.Clear();
            var previousPool = _pool;
            _pool = null;
            previousPool.Dispose();
            
            _pool = new ObjectPool<PoolObject>(
                createFunc: () =>
                {
                    var poolObject = _gameObjectFactory.InstantiatePrefab(prefab.gameObject, _parentForPoolObjects).GetComponent<PoolObject>();
                    poolObject.SetPrefabReference(prefab);
                    return poolObject;
                }, 
                actionOnGet: (monoBehaviour) => monoBehaviour.gameObject.SetActive(true), 
                actionOnRelease: (monoBehaviour) => monoBehaviour.gameObject.SetActive(false), 
                actionOnDestroy: (monoBehaviour) => _destroyer.DestroyGameObject(monoBehaviour.gameObject), 
                collectionCheck, defaultCapacity, maxSize);
        }

        private PoolObject GetFreeElement()
        {
            var element = _pool.Get();
            return element;
        }

        public PoolObject GetFreeElement(Vector3 position)
        {
            var element = GetFreeElement();
            element.transform.position = position;
            return element;
        }

        public PoolObject GetFreeElement(Vector3 position, Quaternion rotation)
        {
            var element = GetFreeElement(position);
            element.transform.rotation = rotation;
            return element;
        }

        public PoolObject GetFreeElement(Vector3 position, Quaternion rotation, Transform parent)
        {
            var element = GetFreeElement(position, rotation);
            if (parent != null)
            {
                element.transform.SetParent(parent);
            }
            return element;
        }

        public void ReturnToPool(PoolObject poolObject)
        {
            poolObject.transform.SetParent(_parentForPoolObjects);
            _pool.Release(poolObject);
        }
    }
}