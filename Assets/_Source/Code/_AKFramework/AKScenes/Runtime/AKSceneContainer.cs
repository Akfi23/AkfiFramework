using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

#if !UNITY_ADDRESSABLES_EXIST

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [Serializable]
    public class AKSceneContainer : AKTypeContainer
    {
        public AKSceneAsset Scene => _scene;

        [SerializeField]
        private AKSceneAsset _scene;
    }
}
#endif