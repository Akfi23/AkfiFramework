using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public class AKCapsuleTriggerCastEventsProvider : AKTriggerCastEventsProvider
    {
        [SerializeField]
        private Vector3 point1;
        public Vector3 Point1 { get => point1; set => point1 = value; }
        [SerializeField]
        private Vector3 point2 = new(0f, 4f, 0f);
        public Vector3 Point2 { get => point2; set => point2 = value; }
        [SerializeField]
        private float radius = 2f;
        public float Radius { get => radius; set => radius = value; }
        
        protected override void CheckColliders()
        {
            Count = Physics.OverlapCapsuleNonAlloc(transform.TransformPoint(point1), transform.TransformPoint(point2),
                radius, Colliders, layerMask);
        }
    }
}