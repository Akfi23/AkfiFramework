using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    [CustomEditor(typeof(AKBoxTriggerCastEventsProvider))]
    public class AKBoxTriggerEventProviderEditor : AKTriggerEventsProviderEditor
    {
        private readonly BoxBoundsHandle _boundsHandle = new();
        
        protected override void Draw(AKTriggerCastEventsProvider value)
        {
            if(!value.enabled) return;
            if(value is not AKBoxTriggerCastEventsProvider t) return;
            
            var bound = t.Bounds;
            _boundsHandle.center = bound.center;
            _boundsHandle.size = bound.size;
            
            EditorGUI.BeginChangeCheck();
            _boundsHandle.DrawHandle();
            
            var center = bound.center;
            if (Tools.current is not (Tool.Transform or Tool.Move or Tool.Scale))
            {
                center = Handles.PositionHandle(center, Quaternion.identity);
                _boundsHandle.center = center;
                _boundsHandle.DrawHandle();
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(value, "Box Trigger - Change Values");
                bound.center = _boundsHandle.center;
                bound.size = _boundsHandle.size;
                t.Bounds = bound;
                EditorUtility.SetDirty(t);
            }
        }
    }
}