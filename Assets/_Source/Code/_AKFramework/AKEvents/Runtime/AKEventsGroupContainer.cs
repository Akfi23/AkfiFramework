using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEvents.Runtime
{
    [Serializable]
    public class AKEventsGroupContainer : AKTypeContainer
    {
        public AKEventContainer[] EventContianers => eventContianers;
        
        [SerializeField]
        private AKEventContainer[] eventContianers;
    }
}