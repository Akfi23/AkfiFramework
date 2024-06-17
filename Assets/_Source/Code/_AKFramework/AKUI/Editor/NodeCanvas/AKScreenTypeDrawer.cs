using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Editor.NodeCanvas
{
    public class AKScreenTypeDrawer : ObjectDrawer<AKScreen>
    {
        private AKUIDatabase _database;
        private int _index;

        public override AKScreen OnGUI(GUIContent content, AKScreen instance)
        {
            if (instance == null)
            {
                instance = new AKScreen();
            }

            if (_database == null)
            {
                var typeName = nameof(AKUIDatabase);

                var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");

                if (assetsGuids == null || assetsGuids.Length == 0)
                {
                    AKDebug.LogWarning($"Missing Database: {typeName}");
                    return instance;
                }

                var path = AssetDatabase.GUIDToAssetPath(assetsGuids.First());
                _database = AssetDatabase.LoadAssetAtPath<AKUIDatabase>(path);
            }

            if (_database == null) return instance;

            var guidNamePairs = new Dictionary<int, string>();

            foreach (var layer0 in _database.ScreenGroupsContainers)
            {
                foreach (var layer1 in layer0.ScreenContainers)
                {
                    guidNamePairs[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            var guid = instance._Id;

            if (!guidNamePairs.ContainsKey(instance._Id))
            {
                instance = new AKScreen();
            }

            var names = guidNamePairs.Values.ToList();
            names.Insert(0, AKConstants.NONE);

            var name = guidNamePairs.GetValueOrDefault(instance._Id, AKConstants.NONE);


            if (guid == 0)
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
                    instance = new AKScreen(0, AKConstants.NONE);
                }
                else
                {
                    var guidStr = guidNamePairs.Keys.ToArray()[_index - 1];
                    instance = new AKScreen(guidStr, guidNamePairs[guidStr]);
                }
            }

            return instance;
        }
    }
}