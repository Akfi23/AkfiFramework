using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public class AKSphereTriggerCastEventsProvider : AKTriggerCastEventsProvider
    {
        [SerializeField] 
        private Vector3 center;
        public Vector3 Center
        {
            get => center;
            set => center = value;
        }
        [SerializeField]
        private float radius = 2f;

        public float Radius
        {
            get => radius;
            set => radius = value;
        }

        protected override void CheckColliders()
        {
            Count = Physics.OverlapSphereNonAlloc(transform.TransformPoint(Center), radius, Colliders, layerMask);
        }
    }
}