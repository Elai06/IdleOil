using UnityEngine;
using Zenject;

namespace Utils.Pools
{
    public class PoolObject : MonoBehaviour
    {
        private PoolSystem _poolSystem;
        public PoolObject PrefabReference { get; private set; }

        [Inject]
        private void Construct(PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
        }

        private void OnDisable()
        {
            _poolSystem.ReturnObjectToPool(this);
        }

        public void SetPrefabReference(PoolObject poolObject)
        {
            PrefabReference = poolObject;
        }
    }
}