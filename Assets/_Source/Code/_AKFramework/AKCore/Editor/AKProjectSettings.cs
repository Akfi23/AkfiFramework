using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public class AKProjectSettings<T> : ScriptableObject where T : AKProjectSettings<T>
    {
        public static readonly string _assetPath = $"Assets/AKFramework/Editor/{typeof(T).Name}.asset";
        
        public static T GetOrCreateSettings()
        {
            if (!AssetDatabase.IsValidFolder("Assets/AKFramework"))
            {
                AssetDatabase.CreateFolder("Assets", "AKFramework");
                AssetDatabase.Refresh();
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/AKFramework/Editor"))
            {
                AssetDatabase.CreateFolder("Assets/AKFramework", "Editor");
                AssetDatabase.Refresh();
            }
            
          

            var settings = AssetDatabase.LoadAssetAtPath<T>(_assetPath);
            if (settings == null)
            {
                settings = CreateInstance<T>();
                AssetDatabase.CreateAsset(settings, _assetPath);
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