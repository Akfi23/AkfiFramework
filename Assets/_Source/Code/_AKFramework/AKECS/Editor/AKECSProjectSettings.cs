using UnityEditor;
using UnityEngine;

namespace _Source.Code._Core.ECS.Generator
{
    [CreateAssetMenu(fileName = "AKECSProjectSettings", menuName = "CORE/SETTINGS/ECS_SETTINGS", order = 1)]
    public class AKECSProjectSettings : ScriptableObject
    {
        public const string settingsPath = "Assets/AKFramework/Editor/AKECSProjectSettings.asset";

        public bool GenerateAuthorings => generateAuthorings;
        
        public string AuthoringsFilePath => authoringsFilePath;

        public string AuthoringsNamespace => authoringsNamespace;

        public string ComponentsNamespace => componentsNamespace;

        [SerializeField]
        private bool generateAuthorings = false;

        [SerializeField]
        private string authoringsFilePath;

        [SerializeField]
        private string authoringsNamespace;

        [SerializeField]
        private string componentsNamespace;

        internal static AKECSProjectSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<AKECSProjectSettings>(settingsPath);
            if (settings == null)
            {
                settings = CreateInstance<AKECSProjectSettings>();
                
                if (settings.authoringsFilePath == string.Empty)
                    settings.authoringsFilePath = @"_Source/Code/Authorings";

                if (settings.authoringsNamespace == string.Empty)
                    settings.authoringsNamespace = "_Source.Code.Authorings";

                if (settings.componentsNamespace == string.Empty)
                    settings.componentsNamespace = "_Source.Code.Components";

                AssetDatabase.CreateAsset(settings, settingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}