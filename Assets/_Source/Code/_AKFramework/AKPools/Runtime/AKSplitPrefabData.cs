using System;
using _Source.Code._AKFramework.AKScenes.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Pools/Split Prefab Data")]
    [InlineEditor]
    public class AKSplitPrefabData : AKPrefabData
    {
        public AKPoolSceneData[] PoolSceneData => _poolSceneData;

        [SerializeField] 
        private AKPoolSceneData[] _poolSceneData;
    }

    [Serializable]
    public class AKPoolSceneData
    {
        [SerializeField]
        private AKScene _scene;
        [SerializeField]
        private bool _customSettings = false;
        [SerializeField]
        [ShowIf("_customSettings")]
        private int _maxPoolSize = 10;
        [SerializeField]
        [ShowIf("_customSettings")]
        private int _defaultCapacity = 10;
        [SerializeField] 
        [ShowIf("_customSettings")]
        private int _spawnOnInitCount = 0;

        public AKScene Scene => _scene;
        public bool CustomSettings => _customSettings;
        public int MaxPoolSize => _maxPoolSize;
        public int DefaultCapacity => _defaultCapacity;
        public int SpawnOnInitCount => _spawnOnInitCount;
    }
}