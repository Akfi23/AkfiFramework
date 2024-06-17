using System;
using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Pools Database", fileName = "db_pools")]
    public class AKPoolsDatabase : AKDatabase
    {
        public AKPrefabsGroupContainer[] PrefabsGroupContainers => _prefabsGroupContainers;

        [SerializeField]
        private AKPrefabsGroupContainer[] _prefabsGroupContainers = Array.Empty<AKPrefabsGroupContainer>();

        public override string Title => "Pools";
        
        
#if UNITY_EDITOR

        public override void UpdateAKType()
        {
            foreach (var group in PrefabsGroupContainers)
            {
                foreach (var prefab in group.PrefabContainers)
                {
                    AKTypeUtilsUpdater.AddID($"{group._Name}/{prefab._Name}", prefab._Id);
                }

                AKTypeUtilsUpdater.AddID($"{group._Name}", group._Id);
            }
        }

        [Button]
        private void UpdateAKPrefab()
        {
            UpdateAKType();
            AKTypeUtilsUpdater.UpdateID();
        }
#endif

        
        protected override void Generate(out AKGenerationData[] generationData)
        {
            var prefabs = new Dictionary<int, string>();

            foreach (var layer0 in _prefabsGroupContainers)
            {
                foreach (var layer1 in layer0.PrefabContainers)
                {
                    prefabs[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            generationData = new[]
            {
                new AKGenerationData
                {
                    FileName = "AKPrefabs",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKPools.Runtime;",
                    },
                    AKType = typeof(AKPrefab),
                    Properties = prefabs
                }
            };
        }
    }
}