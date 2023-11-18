using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public static class AKEditorExtensions
    {
       
        [Flags]
        public enum FindOptions
        {
            None = 0,
            ConsiderChildren = 1
        }

        public static List<T> FindAssets<T>(string[] folders = null) where T : Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T)}", folders);
            return guids.Select(g =>
                UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(g))).ToList();
        }

        public static List<GameObject> FindPrefabs<T>(FindOptions options = 0, string[] folders = null)
        {
            return FindPrefabs(new[] { typeof(T) }, options, folders);
        }

        public static List<GameObject> FindPrefabs<T1, T2>(FindOptions options = 0, string[] folders = null)
        {
            return FindPrefabs(new[] { typeof(T1), typeof(T2) }, options, folders);
        }

        public static List<GameObject> FindPrefabs<T1, T2, T3>(FindOptions options = 0, string[] folders = null)
        {
            return FindPrefabs(new[] { typeof(T1), typeof(T2), typeof(T3) }, options, folders);
        }

        public static List<GameObject> FindPrefabs(IEnumerable<Type> types, FindOptions options, string[] folders)
        {
            var considerChildren = options.HasFlag(FindOptions.ConsiderChildren);

            var guids = UnityEditor.AssetDatabase.FindAssets("t:Prefab", folders);

            var prefabs = guids.Select(g => UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                UnityEditor.AssetDatabase.GUIDToAssetPath(g))).ToList();

            if (considerChildren)
            {
                IEnumerable<GameObject> result = prefabs;

                foreach (var type in types)
                {
                    result = result.Where(p => p.GetComponentInChildren(type) != null);
                }

                return result.ToList();
            }
            else
            {
                IEnumerable<GameObject> result = prefabs;

                foreach (var type in types)
                {
                    result = result.Where(p => p.GetComponent(type) != null);
                }

                return result.ToList();
            }
        }

        public static string[] FindAssetsPath<T>(IEnumerable<T> obj) where T : Object
        {
            var enumerable = obj.ToList();
            var paths = new string[enumerable.Count()];
            var i = 0;
            foreach (var o in enumerable)
            {
                paths[i] = UnityEditor.AssetDatabase.GetAssetPath(o);
                i++;
            }

            return paths;
        }

        public static void CreateFolder( string assetPath)
        {
            string directoryPath = Path.GetDirectoryName(assetPath);
            if (Directory.Exists(directoryPath))
                return;
            Directory.CreateDirectory(directoryPath);
            AssetDatabase.Refresh();
        }
    }
}