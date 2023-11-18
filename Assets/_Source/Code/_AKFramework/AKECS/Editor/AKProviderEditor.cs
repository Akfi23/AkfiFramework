using _Source.Code._AKFramework.AKECS.Runtime;
using UnityEditor;

namespace _Source.Code._Core.ECS.Generator
{
    
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AKProvider<>), true)]
    public  class AKProviderEditor:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var property = serializedObject.GetIterator();

            EditorGUI.BeginChangeCheck();

            while (property.NextVisible(true))
            {
                if (property.name != "m_Script" && property.depth == 1)
                {
                    EditorGUILayout.PropertyField(property);
                }

                if (property.name != "m_Script" && property.depth == 0 && property.name != "_value")
                {
                    EditorGUILayout.PropertyField(property);
                }
            }

            property.Reset();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        } 
    }
}