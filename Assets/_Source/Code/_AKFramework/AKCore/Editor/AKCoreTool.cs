using System;
using Sirenix.OdinInspector;
using UnityEditor;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    [Serializable]
    public sealed class AKCoreTool : IAKEditorTool
    {
        [Button]
        private static void GenerateScripts()
        {
            EditorUtility.DisplayProgressBar("Scripts Generation", "Wait...", 0);

            var guids = AssetDatabase.FindAssets($"t:{nameof(AKDatabase)}");

            var databaseCodeGenerator = new AKDatabaseCodeGenerator();
            
            foreach (var guid in guids)
            {
                var database = AssetDatabase.LoadAssetAtPath<AKDatabase>(AssetDatabase.GUIDToAssetPath(guid));
                database.GetGenerationData(out var generationData);
                databaseCodeGenerator.Generate(generationData);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.ClearProgressBar();
        }

        [Button]
        private static void ResetAllDatabase()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(AKDatabase)}");

            foreach (var guid in guids)
            {
                var database = AssetDatabase.LoadAssetAtPath<AKDatabase>(AssetDatabase.GUIDToAssetPath(guid));
                database.ResetData();
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public string Title => "Core";
    }
}