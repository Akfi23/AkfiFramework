using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public class AKCoreProjectSettings : AKProjectSettings<AKCoreProjectSettings>
    {
        public SceneAsset ContextRootScene => contextRootScene;

        public string GeneratorScriptsPath => generatorScriptsPath;


        [SerializeField]
        private SceneAsset contextRootScene;
        
        [SerializeField]
        private string generatorScriptsPath = "AKFramework/Generated";
    }
}