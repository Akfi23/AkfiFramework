using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    [Serializable]
    public class AKPrefabsGroupContainer : AKTypeContainer
    {
        public AKPrefabContainer[] PrefabContainers => _prefabContainers;

        [SerializeField]
        private AKPrefabContainer[] _prefabContainers = Array.Empty<AKPrefabContainer>();
    }
}