using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    [Serializable]
    public class AKTagsGroupContainer : AKTypeContainer
    {
        public AKTagContainer[] Tags => _tags;

        [SerializeField]
        private AKTagContainer[] _tags;
    }
}