using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKEvents.Runtime;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEvents.Editor.NodeCanvas
{
    public class AKEventTypeDrawer : ObjectDrawer<AKEvent>
    {
        private AKEventsDatabase _database;
        private int _index;

        public override AKEvent OnGUI(GUIContent content, AKEvent instance)
        {
            if (instance == null)
            {
                instance = new AKEvent();
            }

            if (_database == null)
            {
                var typeName = nameof(AKEventsDatabase);

                var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");

                if (assetsGuids == null || assetsGuids.Length == 0)
                {
                    Debug.LogWarning($"Missing Database: {typeName}");
                    return instance;
                }

                var path = AssetDatabase.GUIDToAssetPath(assetsGuids.First());
                _database = AssetDatabase.LoadAssetAtPath<AKEventsDatabase>(path);
            }

            if (_database == null) return instance;

            var guidNamePairs = new Dictionary<string, string>();

            foreach (var layer0 in _database.EventsGroupContainers)
            {
                foreach (var layer1 in layer0.EventContianers)
                {
                    guidNamePairs[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            var guid = instance._Id;

            if (!guidNamePairs.ContainsKey(instance._Id.ToString()))
            {
                instance = new AKEvent();
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
                    instance = new AKEvent(string.Empty, AKConstants.NONE);
                }
                else
                {
                    var guidStr = guidNamePairs.Keys.ToArray()[_index - 1];
                    instance = new AKEvent(guidStr, guidNamePairs[guidStr]);
                }
            }

            return instance;
        }
    }
}