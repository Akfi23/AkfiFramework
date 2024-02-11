using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    [Serializable]
    public class AKPrefabContainer : AKTypeContainer
    {
        public AKPrefabData PrefabData => _prefabData;
        
        [Required]
        [SerializeField]
        private AKPrefabData _prefabData;
    }
}