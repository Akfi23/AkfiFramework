using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKEvents.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEvents.Editor
{
    [CustomPropertyDrawer(typeof(AKEvent))]
    public class AKEventDrawer : AKTypeDrawer<AKEvent, AKEventsDatabase>
    {
        protected override void InitializeAKTypeDrawer(ref Rect position, AKEventsDatabase database,
            SerializedProperty property,
            GUIContent label)
        {
            var resultLayers = new Dictionary<int, string>();

            foreach (var layer0 in database.EventsGroupContainers)
            {
                foreach (var layer1 in layer0.EventContianers)
                {
                    resultLayers.Add(layer1._Id, $"{layer0._Name}/{layer1._Name}");
                }
            }


            DrawAKTypeProperty(ref position, property, label, resultLayers);
        }

    }
}