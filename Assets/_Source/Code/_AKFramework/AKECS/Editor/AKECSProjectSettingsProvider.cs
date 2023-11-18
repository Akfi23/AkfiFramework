using System.Collections.Generic;
using UnityEditor;

namespace _Source.Code._Core.ECS.Generator
{
    public static class AKECSProjectSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider Create()
        {
            var provider = new SettingsProvider("Project/AKFramework/ECS", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    var settings = AKECSProjectSettings.GetSerializedSettings();

                    EditorGUILayout.PropertyField(settings.FindProperty("generateAuthorings"));
                    EditorGUILayout.PropertyField(settings.FindProperty("authoringsFilePath"));
                    EditorGUILayout.PropertyField(settings.FindProperty("authoringsNamespace"));
                    EditorGUILayout.PropertyField(settings.FindProperty("componentsNamespace"));
                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "ECS", "Authoring", "Path" })
            };

            return provider;
        }
    }
}