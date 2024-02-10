using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    [CustomEditor(typeof(_AKSphereTriggerCastEvent))]
    public class AKSphereTriggerEventEditor : OdinEditor
    {
        private readonly SphereBoundsHandle _boundsHandle = new();
        
        public void OnSceneGUI()
        {
            var triggerEventsProvider = (_AKSphereTriggerCastEvent)target;
            
            Handles.color = Color.green;
            var currentMatrix = Handles.matrix;

            if (triggerEventsProvider != null)
            {
                Handles.matrix = triggerEventsProvider.transform.localToWorldMatrix;
                Draw(triggerEventsProvider);
            }

            Handles.matrix = currentMatrix;
        }

        
        private void Draw(_AKSphereTriggerCastEvent value)
        {
            if(!value.enabled) return;

            _boundsHandle.center = value.TargetValue.center;
            _boundsHandle.radius = value.TargetValue.radius;
            
            EditorGUI.BeginChangeCheck();
            _boundsHandle.DrawHandle();
            var center = value.TargetValue.center;
            var radius = value.TargetValue.radius;
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
                var targetValue = value.TargetValue;
                targetValue.center = _boundsHandle.center;
                targetValue.radius = _boundsHandle.radius;
                EditorUtility.SetDirty(value);
            }
        }
    }
}