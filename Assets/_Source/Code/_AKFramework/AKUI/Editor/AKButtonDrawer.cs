using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Editor
{
    [CustomPropertyDrawer(typeof(AKButton))]
    public class AKButtonDrawer : AKTypeDrawer<AKButton, AKUIDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKUIDatabase database,
            SerializedProperty property,
            GUIContent label)
        {
            var resultLayers = new Dictionary<int, string>();

            foreach (var layer0 in database.ScreenGroupsContainers)
            {
                foreach (var layer1 in layer0.ScreenContainers)
                {
                    foreach (var layer2 in layer1.ButtonContainers)
                    {
                        resultLayers.Add(layer2._Id, $"{layer0._Name}/{layer1._Name}/{layer2._Name}");
                    }
                }
            }

            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }
    }
}