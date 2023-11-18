using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Editor
{
    [CustomPropertyDrawer(typeof(AKTag))]
    public class AKTagDrawer : AKTypeDrawer<AKTag,AKTagsDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKTagsDatabase database, SerializedProperty property, GUIContent label)
        {
            var resultLayers = new Dictionary<string, string>();

            foreach (var layer0 in database.Groups)
            {
                foreach (var layer1 in layer0.Tags)
                {
                    resultLayers.Add(layer1._Id, $"{layer0._Name}/{layer1._Name}");
                }
            }

            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }
    }
}
