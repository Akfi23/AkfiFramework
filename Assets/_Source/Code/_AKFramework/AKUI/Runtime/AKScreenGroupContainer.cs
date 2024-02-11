using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [Serializable]
    public sealed class AKScreenGroupContainer : AKTypeContainer
    {
        public AKScreenContainer[] ScreenContainers => screenContainers;
        
        [SerializeField]
        private AKScreenContainer[] screenContainers;
    }
}