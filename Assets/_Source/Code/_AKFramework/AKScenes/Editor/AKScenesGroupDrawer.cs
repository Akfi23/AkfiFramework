using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKScenes.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKScenes.Editor
{
    [CustomPropertyDrawer(typeof(AKScenesGroup))]
    public class AKScenesGroupDrawer : AKTypeDrawer<AKScenesGroup,AKScenesDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKScenesDatabase database,
            SerializedProperty property,
            GUIContent label)
        {
            var resultLayers = new Dictionary<int, string>();

            foreach (var layer0 in database.Groups)
            {
                resultLayers.Add(layer0._Id, $"{layer0._Name}");
            }

            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }
    }
}