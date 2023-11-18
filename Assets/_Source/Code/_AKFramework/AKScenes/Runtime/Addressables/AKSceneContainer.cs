#if UNITY_ADDRESSABLES_EXIST
using System;
using _Source.Code._Core.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AddressableAssets;

// ReSharper disable once CheckNamespace
namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [Serializable]
    public class AKSceneContainer : AKTypeContainer
    {
        public AssetReference Scene => _scene;
        
        [FormerlySerializedAs("sceneReference")]
        [SerializeField]
        private AssetReference _scene;
    }
}
#endif