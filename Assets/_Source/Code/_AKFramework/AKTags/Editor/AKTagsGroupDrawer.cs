using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Editor
{
    [CustomPropertyDrawer(typeof(AKTagsGroup))]
    public class AKTagsGroupDrawer:AKTypeDrawer<AKTagsGroup, AKTagsDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKTagsDatabase database,
            SerializedProperty property,
            GUIContent label)
        {
            var resultLayers = new Dictionary<string, string>();

            foreach (var layer0 in database.Groups)
            {
                resultLayers.Add(layer0._Id, $"{layer0._Name}");
            }

            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }
    }
}