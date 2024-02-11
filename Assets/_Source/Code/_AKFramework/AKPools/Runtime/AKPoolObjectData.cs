using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    public abstract class AKPoolObjectData : ScriptableObject
    {
        public bool CollectionChecks => _collectionChecks;

        public int MaxPoolSize => _maxPoolSize;

        public int DefaultCapacity => _defaultCapacity;

        public int SpawnOnInitCount => _spawnOnInitCount;
        
        [SerializeField]
        private bool _collectionChecks = true;

        [SerializeField]
        private int _maxPoolSize = 10;

        [SerializeField]
        private int _defaultCapacity = 10;

        [SerializeField] 
        private int _spawnOnInitCount = 0;
    }
}