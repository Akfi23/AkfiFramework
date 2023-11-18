using System;
using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public abstract class AKTypeDrawer<T,D> : PropertyDrawer where T : AKType where D: AKDatabase
    {
        private SerializedProperty _pathProperty;
        private SerializedProperty _guidProperty;

        private D _database;
        private int _index;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_database == null)
            {
                var typeName = typeof(D).Name;

                var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");

                if (assetsGuids == null || assetsGuids.Length == 0)
                {
                    Debug.LogWarning($"Missing database: {typeName}");
                    return;
                }

                var path = AssetDatabase.GUIDToAssetPath(assetsGuids.First());
                _database = AssetDatabase.LoadAssetAtPath<D>(path);
            }

            if (_database == null) return;

            InitializeAKTypeDrawer(ref position, _database, property, label);
        }


        protected abstract void InitializeAKTypeDrawer(ref Rect position, D database, SerializedProperty property,
            GUIContent label);


        protected void DrawAKTypeProperty(ref Rect position, SerializedProperty property, GUIContent label,
            Dictionary<string, string> guidNamePairs)
        {
            _guidProperty = property.FindPropertyRelative("id");
            _pathProperty = property.FindPropertyRelative("name");

            if (string.IsNullOrEmpty(_guidProperty.stringValue))
            {
                _guidProperty.stringValue = AKConstants.EmptyGUIDString;
                _pathProperty.stringValue = AKConstants.NONE;
            }

            var guid = Guid.Parse(_guidProperty.stringValue);


            if (!guidNamePairs.ContainsKey(_guidProperty.stringValue))
            {
                _guidProperty.stringValue = AKConstants.EmptyGUIDString;
                _pathProperty.stringValue = AKConstants.NONE;
            }

            var names = guidNamePairs.Values.ToList();
            names.Insert(0, AKConstants.NONE);

            var name = guidNamePairs.ContainsKey(_guidProperty.stringValue)
                ? guidNamePairs[_guidProperty.stringValue]
                : AKConstants.NONE;

            var isNoneDropdown = guid == Guid.Empty;

            _index = names.IndexOf(name);

            EditorGUI.BeginChangeCheck();

            if (isNoneDropdown)
            {
                GUI.backgroundColor = Color.red;
            }

            _index = EditorGUI.Popup(position, label.text, _index, names.ToArray());

            GUI.backgroundColor = Color.white;

            if (EditorGUI.EndChangeCheck())
            {
                if (_index == 0)
                {
                    _guidProperty.stringValue = AKConstants.EmptyGUIDString;
                    _pathProperty.stringValue = AKConstants.NONE;
                }
                else
                {
                    var guidStr = guidNamePairs.Keys.ToArray()[_index - 1];
                    _pathProperty.stringValue = guidNamePairs[guidStr];
                    _guidProperty.stringValue = guidNamePairs.Keys.ToArray()[_index - 1];
                }
            }
        }  
    }
}