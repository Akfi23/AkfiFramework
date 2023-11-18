using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [Serializable]
    public class AKSceneAsset
    {
        [SerializeField]
        private Object _sceneAsset;

        [SerializeField]
        private string _sceneName = "";
        
        [SerializeField]
        private string _scenePath = "";

        public string SceneName => _sceneName;

        public string ScenePath => _scenePath;

        public static implicit operator string(AKSceneAsset sceneAsset)
        {
            return sceneAsset._scenePath;
        }
    }
}