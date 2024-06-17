#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
namespace _Source.Code._AKFramework.AKCore.Runtime
{
    public static class AKTypeUtilsUpdater
    {
        private static readonly Dictionary<string, int> TypeMapping = new();

        public static void AddID(string name, int id)
        {
            if (TypeMapping.ContainsKey(name))
            {
                TypeMapping.Remove(name);
            }
            TypeMapping.Add(name, id);
        }
        
        public static void UpdateID()
        {
            var assetPaths = AssetDatabase.GetAllAssetPaths();
            var count = 0;
            foreach (var path in assetPaths)
            {
                count++;
                
                if(path.IndexOf("Assets/", StringComparison.Ordinal) != 0) continue;
                if(!path.Contains(".prefab") && !path.Contains(".asset") && !path.Contains(".unity")) continue;

                EditorUtility.DisplayProgressBar("AKType updates", $"Update {path}", (float) count / assetPaths.Length);

                using (var reader = File.OpenText(path))
                {
                    var textAll = reader.ReadToEnd();
                    var text = textAll.Split("\n");
                    var isContains = false;
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (!text[i].Contains($"name: ")) continue;
                        if (!text[i + 1].Contains("id: ")) continue;

                        if (!GetTypeId(text[i], out var id)) continue;

                        isContains = true;
                        var index = text[i + 1].LastIndexOf(": ", StringComparison.Ordinal) + 2;
                        text[i + 1] = text[i + 1].Remove(index, text[i + 1].Length - index);
                        text[i + 1] += $"{id}";
                    }

                    if (!isContains) continue;

                    var tmp = "";
                    for(int i = 0; i < text.Length; i++)
                    {
                        tmp += text[i];
                        if (i < text.Length - 1) tmp += "\n";
                    }

                    reader.Close();

                    File.WriteAllText(path, tmp);
                }
            }
            
            TypeMapping.Clear();
            EditorUtility.ClearProgressBar();
        }

        private static bool GetTypeId(string text, out int id)
        {
            foreach (var nameToId in TypeMapping)
            {
                if(text != $"    - name: {nameToId.Key}" && text != $"      name: {nameToId.Key}") continue;

                id = nameToId.Value;
                return true;
            }

            id = 0;
            return false;
        }
    }
}
#endif