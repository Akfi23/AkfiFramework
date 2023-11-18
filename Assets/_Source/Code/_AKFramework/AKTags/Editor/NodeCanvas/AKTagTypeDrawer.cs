using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Editor.NodeCanvas
{
    public class AKTagTypeDrawer : ObjectDrawer<AKTag>
    {
        private AKTagsDatabase _database;
        private int _index;

        public override AKTag OnGUI(GUIContent content, AKTag instance)
        {
            if (instance == null)
            {
                instance = new AKTag();
            }

            if (_database == null)
            {
                var typeName = nameof(AKTagsDatabase);

                var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");

                if (assetsGuids == null || assetsGuids.Length == 0)
                {
                    Debug.LogWarning($"Missing Database: {typeName}");
                    return instance;
                }

                var path = AssetDatabase.GUIDToAssetPath(assetsGuids.First());
                _database = AssetDatabase.LoadAssetAtPath<AKTagsDatabase>(path);
            }

            if (_database == null) return instance;

            var guidNamePairs = new Dictionary<string, string>();

            foreach (var layer0 in _database.Groups)
            {
                foreach (var layer1 in layer0.Tags)
                {
                    guidNamePairs[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            var guid = instance._Id;

            if (!guidNamePairs.ContainsKey(instance._Id.ToString()))
            {
                instance = new AKTag();
            }

            var names = guidNamePairs.Values.ToList();
            names.Insert(0, AKConstants.NONE);

            var name = guidNamePairs.ContainsKey(instance._Id)
                ? guidNamePairs[instance._Id]
                : AKConstants.NONE;


            if (string.IsNullOrWhiteSpace(guid))
            {
                GUI.backgroundColor = Color.red;
            }

            _index = names.IndexOf(name);

            EditorGUI.BeginChangeCheck();

            _index = EditorGUILayout.Popup(content, _index, names.ToArray());

            GUI.backgroundColor = Color.white;

            if (EditorGUI.EndChangeCheck())
            {
                if (_index == 0)
                {
                    instance = new AKTag(string.Empty, AKConstants.NONE);
                }
                else
                {
                    var guidStr = guidNamePairs.Keys.ToArray()[_index - 1];
                    instance = new AKTag(guidStr, guidNamePairs[guidStr]);
                }
            }

            return instance;
        }
    }
}