using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    [CustomEditor(typeof(AKSphereTriggerCastEventsProvider))]
    public class AKSphereTriggerEventsProviderEditor : AKTriggerEventsProviderEditor
    {
        private readonly SphereBoundsHandle _boundsHandle = new();
        
        protected override void Draw(AKTriggerCastEventsProvider value)
        {
            if(!value.enabled) return;
            if(value is not AKSphereTriggerCastEventsProvider t) return;

            _boundsHandle.center = t.Center;
            _boundsHandle.radius = t.Radius;
            
            EditorGUI.BeginChangeCheck();
            _boundsHandle.DrawHandle();
            var center = t.Center;
            var radius = t.Radius;
            if (Tools.current is not (Tool.Transform or Tool.Move or Tool.Scale))
            {
                center = Handles.PositionHandle(center, Quaternion.identity);
                radius = Handles.RadiusHandle(Quaternion.identity, center, radius);
                _boundsHandle.center = center;
                _boundsHandle.radius = radius;
                _boundsHandle.DrawHandle();
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(value, "Sphere Trigger - Change Values");
                t.Center = _boundsHandle.center;
                t.Radius = _boundsHandle.radius;
                EditorUtility.SetDirty(t);
            }
        }
    }
}