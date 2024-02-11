using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using SFramework.UI.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [Serializable]
    public sealed class AKButtonContainer : AKTypeContainer
    {
        public AKButtonData ButtonData => buttonData;

        [SerializeField] private AKButtonData buttonData;
    }
}