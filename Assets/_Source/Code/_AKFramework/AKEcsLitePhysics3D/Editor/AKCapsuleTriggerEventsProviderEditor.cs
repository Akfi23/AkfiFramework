using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using UnityEditor;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    [CustomEditor(typeof(AKCapsuleTriggerCastEventsProvider))]
    public class AKCapsuleTriggerEventsProviderEditor : AKTriggerEventsProviderEditor
    {
        
        protected override void Draw(AKTriggerCastEventsProvider value)
        {
            if(!value.enabled) return;
            if(value is not AKCapsuleTriggerCastEventsProvider t) return;
            
            var pos = t.transform.position;
            Undo.RecordObject(value, "Capsule Trigger - Change Points");
            t.Point1 = Handles.PositionHandle(t.Point1, Quaternion.identity);
            t.Point2 = Handles.PositionHandle(t.Point2, Quaternion.identity);
            t.Radius = Handles.ScaleSlider(t.Radius, Vector3.Lerp(t.Point1, t.Point2, 0.5f), Vector3.right,
                Quaternion.identity, 1f, 1f);
            DrawWireCapsule(pos + t.Point1, pos + t.Point2, t.Radius);
            EditorUtility.SetDirty(t);
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