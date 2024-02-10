using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    [CustomEditor(typeof(_AKBoxTriggerCastEvent))]
    public class AKBoxTriggerEventEditor : OdinEditor
    {
        private readonly BoxBoundsHandle _boundsHandle = new();
        
        public void OnSceneGUI()
        {
            var triggerEventsProvider = target as _AKBoxTriggerCastEvent;
            if(triggerEventsProvider == null) return;
            
            Handles.color = Color.green;
            var currentMatrix = Handles.matrix;

            if (triggerEventsProvider != null)
            {
                Handles.matrix = triggerEventsProvider.transform.localToWorldMatrix;
                Draw(triggerEventsProvider);
            }

            Handles.matrix = currentMatrix;
        }

        
        private void Draw(_AKBoxTriggerCastEvent value)
        {
            if(!value.enabled) return;
            
            var bound = value.TargetValue.bounds;
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
                var targetValue = value.TargetValue;
                targetValue.bounds = bound;
                value.TargetValue = targetValue;
                EditorUtility.SetDirty(value);
            }
        }
    }
}