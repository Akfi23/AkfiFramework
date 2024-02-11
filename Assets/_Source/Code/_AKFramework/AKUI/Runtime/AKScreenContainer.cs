using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using SFramework.UI.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [Serializable]
    public sealed class AKScreenContainer : AKTypeContainer
    {
        public AKButtonContainer[] ButtonContainers => buttonContainers;
        public AKToggleContainer[] ToggleContainers => toggleContainers;
        
        [SerializeField]
        private AKButtonContainer[] buttonContainers;
        
        [SerializeField]
        private AKToggleContainer[] toggleContainers;
    }
}