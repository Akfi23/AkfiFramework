using System;
using System.Collections.Generic;
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
        
        
        protected override void Generate(out AKGenerationData[] generationData)
        {
            var prefabs = new Dictionary<string, string>();

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
                        "_Source.Code._AKFramework.AKPools.Runtime",
                    },
                    AKType = typeof(AKPrefab),
                    Properties = prefabs
                }
            };
        }
    }
}