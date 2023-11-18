using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public static class AKEditorUtils
    {
        public static AKDatabase CreateDatabaseAsset(Type type, bool selectAsset = false,
            bool refreshAssets = false)
        {
            var instance = (AKDatabase) ScriptableObject.CreateInstance(type);

            var path = EditorUtility.SaveFilePanel(
                "Create new database",
                "Assets",
                $"{type.Name}.asset",
                "asset");

            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
            }

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            if (refreshAssets)
                AssetDatabase.Refresh();
            if (selectAsset)
                Selection.activeObject = instance;

            AKDebug.Log("Initialized " + type.Name + "\nPath: " + path);

            return instance;
        }

        public static T CreateDatabaseAsset<T>(bool selectAsset = false, bool refreshAssets = false)
            where T : AKDatabase
        {
            CreateDatabaseAsset(typeof(T), selectAsset, refreshAssets);
            return LoadDatabaseAsset<T>();
        }

        public static T LoadDatabaseAsset<T>() where T : AKDatabase
        {
            var typeName = typeof(T).Name;

            var modulesGUIDS = AssetDatabase.FindAssets($"t:{typeName}");

            if (modulesGUIDS == null || modulesGUIDS.Length == 0)
                return CreateDatabaseAsset<T>();

            var path = AssetDatabase.GUIDToAssetPath(modulesGUIDS.First());
            var module = AssetDatabase.LoadAssetAtPath<T>(path);

            return module;
        }

        public static AKDatabase LoadDatabaseAsset(Type type)
        {
            var modulesGUIDS = AssetDatabase.FindAssets($"t:{type.Name}");

            if (modulesGUIDS == null || modulesGUIDS.Length == 0)
                return CreateDatabaseAsset(type);

            var path = AssetDatabase.GUIDToAssetPath(modulesGUIDS.First());
            var module = AssetDatabase.LoadAssetAtPath<AKDatabase>(path);

            return module;
        }

        public static List<T> LoadScriptableObjectAssets<T>() where T : ScriptableObject
        {
            var type = typeof(T);
            var typeName = $"t:{type.Name}";

            var assets = AssetDatabase.FindAssets(typeName);

            var sfDataList = new List<T>();

            foreach (var guid in assets)
            {
                var sfData = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                sfDataList.Add(sfData);
            }

            return sfDataList;
        }
    }
}