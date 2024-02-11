using System;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKPools.Runtime;
using Sirenix.OdinInspector;

namespace _Source.Code.Objects
{
    [Serializable]
    public struct SpawnData
    {
        public AKPrefab[] prefabs;
        public bool isCountRange;
        public int count;
        [ShowIf("isCountRange")]
        public int maxCount;
        public float spawnRadius;
        public float offsetY;
        public float delayBetweenSpawns;
        public bool withAnim;
        [ShowIf("withAnim")]
        public SpawnAnimData animData;
    }
}