using System.Collections.Generic;
using UnityEditor;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public class AKCoreProjectSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider Create()
        {
            var provider = new SettingsProvider("Project/AKFramework/Core", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    var settings = AKCoreProjectSettings.GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("contextRootScene"));
                    EditorGUILayout.PropertyField(settings.FindProperty("generatorScriptsPath"));
                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "AK", "Core", "AKFramework" })
            };

            return provider;
        }
    }
}