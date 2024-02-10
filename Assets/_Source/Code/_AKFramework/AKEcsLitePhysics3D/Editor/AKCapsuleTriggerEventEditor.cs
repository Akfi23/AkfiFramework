using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    [CustomEditor(typeof(_AKCapsuleTriggerCastEvent))]
    public class AKCapsuleTriggerEventEditor : OdinEditor
    {
        public void OnSceneGUI()
        {
            var triggerEventsProvider = (_AKCapsuleTriggerCastEvent)target;
            
            Handles.color = Color.green;
            var currentMatrix = Handles.matrix;

            if (triggerEventsProvider != null)
            {
                Handles.matrix = triggerEventsProvider.transform.localToWorldMatrix;
                Draw(triggerEventsProvider);
            }

            Handles.matrix = currentMatrix;
        }

        
        private void Draw(_AKCapsuleTriggerCastEvent value)
        {
            if(!value.enabled) return;

            var targetValue = value.TargetValue;
            var pos = value.transform.position;
            Undo.RecordObject(value, "Capsule Trigger - Change Points");
            EditorGUI.BeginChangeCheck();
            
            targetValue.point1 = Handles.PositionHandle(targetValue.point1, Quaternion.identity);
            targetValue.point2 = Handles.PositionHandle(targetValue.point2, Quaternion.identity);
            targetValue.radius = Handles.ScaleSlider(targetValue.radius, Vector3.Lerp(targetValue.point1, targetValue.point2, 0.5f), Vector3.right,
                Quaternion.identity, 1f, 1f);
            DrawWireCapsule(pos + targetValue.point1, pos + targetValue.point2, targetValue.radius);
            if (EditorGUI.EndChangeCheck())
            {
                value.TargetValue = targetValue;
                EditorUtility.SetDirty(value);
            }
        }

        private void DrawWireCapsule(Vector3 pos, Vector3 pos2, float radius)
        {
            var forward = pos2 - pos;
            var rot = Quaternion.LookRotation(forward);
            var pointOffset = radius * 0.5f;
            var length = forward.magnitude;
            var center2 = new Vector3(0f, 0, length);

            var angleMatrix = Matrix4x4.TRS(pos, rot, Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
                Handles.DrawWireDisc(center2, Vector3.forward, radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);

                DrawLine(radius, 0f, length);
                DrawLine(-radius, 0f, length);
                DrawLine(0f, radius, length);
                DrawLine(0f, -radius, length);
            }
        }

        private void DrawLine(float arg1, float arg2, float forward)
        {
            Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
        }
    }
}