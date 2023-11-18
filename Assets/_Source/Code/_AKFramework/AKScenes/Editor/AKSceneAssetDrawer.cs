using _Source.Code._AKFramework.AKScenes.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKScenes.Editor
{
    [CustomPropertyDrawer(typeof(AKSceneAsset))]
    public class AKSceneAssetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);
            var sceneAsset = _property.FindPropertyRelative("_sceneAsset");
            var sceneName = _property.FindPropertyRelative("_sceneName");
            var scenePath = _property.FindPropertyRelative("_scenePath");
            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
            if (sceneAsset != null)
            {
                EditorGUI.BeginChangeCheck();

                sceneAsset.objectReferenceValue = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue,
                    typeof(SceneAsset), false);

                if (EditorGUI.EndChangeCheck())
                {
                    if (sceneAsset.objectReferenceValue != null)
                    {
                        sceneName.stringValue = sceneAsset.objectReferenceValue.name;
                        scenePath.stringValue = AssetDatabase.GetAssetPath(sceneAsset.objectReferenceValue);
                    }
                    else
                    {
                        sceneName.stringValue = string.Empty;
                        scenePath.stringValue = string.Empty;
                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
