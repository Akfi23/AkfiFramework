using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKScenes.Runtime;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKScenes.Editor.NodeCanvas
{
    public class AKSceneTypeDrawer : ObjectDrawer<AKScene>
    {
        private AKScenesDatabase _database;
        private int _index;

        public override AKScene OnGUI(GUIContent content, AKScene instance)
        {
            if (instance == null)
            {
                instance = new AKScene();
            }

            if (_database == null)
            {
                var typeName = nameof(AKScenesDatabase);

                var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");

                if (assetsGuids == null || assetsGuids.Length == 0)
                {
                    AKDebug.LogWarning($"Missing Database: {typeName}");
                    return instance;
                }

                var path = AssetDatabase.GUIDToAssetPath(assetsGuids.First());
                _database = AssetDatabase.LoadAssetAtPath<AKScenesDatabase>(path);
            }

            if (_database == null) return instance;

            var guidNamePairs = new Dictionary<int, string>();

            foreach (var layer0 in _database.Groups)
            {
                foreach (var layer1 in layer0.Scenes)
                {
                    guidNamePairs[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            var guid = instance._Id;

            if (!guidNamePairs.ContainsKey(instance._Id))
            {
                instance = new AKScene();
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
                    instance = new AKScene(0, AKConstants.NONE);
                }
                else
                {
                    var guidStr = guidNamePairs.Keys.ToArray()[_index - 1];
                    instance = new AKScene(guidStr, guidNamePairs[guidStr]);
                }
            }

            return instance;
        }
    }
}