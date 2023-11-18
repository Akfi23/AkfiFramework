using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [Serializable]
    public class AKScenesGroupContainer : AKTypeContainer
    {
        public AKSceneContainer[] Scenes => _scenes;
        
        [SerializeField]
        private AKSceneContainer[] _scenes;
    }
}