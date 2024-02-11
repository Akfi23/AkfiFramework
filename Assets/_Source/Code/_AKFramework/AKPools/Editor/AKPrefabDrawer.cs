using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKPools.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Editor
{
    [CustomPropertyDrawer(typeof(AKPrefab))]
    public class AKPrefabDrawer : AKTypeDrawer<AKPrefab, AKPoolsDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKPoolsDatabase database,
            SerializedProperty property,
            GUIContent label)
        {
            var resultLayers = new Dictionary<string, string>();

            foreach (var layer0 in database.PrefabsGroupContainers)
            {
                foreach (var layer1 in layer0.PrefabContainers)
                {
                    resultLayers.Add(layer1._Id, $"{layer0._Name}/{layer1._Name}");
                }
            }

            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }
    }
}