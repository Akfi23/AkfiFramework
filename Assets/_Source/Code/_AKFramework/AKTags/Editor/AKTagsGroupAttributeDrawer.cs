using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Editor
{
    [CustomPropertyDrawer(typeof(AKTagsGroupAttribute))]
    public class AKTagsGroupAttributeDrawer:AKTypeDrawer<AKTag, AKTagsDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKTagsDatabase database, SerializedProperty property, GUIContent label)
        {
            var groupAttribute = (attribute as AKTagsGroupAttribute)?.groups;

            var resultLayers = new Dictionary<string, string>();

            var group = database.Groups.Where(x => groupAttribute != null && groupAttribute.Contains(x._Name)).ToArray();

            if (group.Length == 0)
            {
                foreach (var layer0 in database.Groups)
                {
                    foreach (var layer1 in layer0.Tags)
                    {
                        resultLayers.Add(layer1._Id, $"{layer0._Name}/{layer1._Name}");
                    }
                }
            }
            else
            {
                foreach (var layer0 in group)
                {
                    foreach (var layer1 in layer0.Tags)
                    {
                        resultLayers.Add(layer1._Id, $"{layer0._Name}/{layer1._Name}");
                    }
                }
            }



            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }
    }
}