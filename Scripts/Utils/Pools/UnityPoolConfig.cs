using UnityEngine;

namespace Utils.Pools
{
    [CreateAssetMenu(fileName = "UnityPoolConfig", menuName = "Configs/UnityPoolConfig", order = 0)]
    public class UnityPoolConfig : ScriptableObject
    {
        [SerializeField] private int _initialCapacity = 10;
        [SerializeField] private int _maxSize = 10000;
        [SerializeField] private bool _collectionCheck;
        
        public int InitialCapacity => _initialCapacity;
        public int MaxSize => _maxSize;
        public bool CollectionCheck => _collectionCheck;
    }
}