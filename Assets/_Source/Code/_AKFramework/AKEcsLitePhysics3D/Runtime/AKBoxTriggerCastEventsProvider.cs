using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public class AKBoxTriggerCastEventsProvider : AKTriggerCastEventsProvider
    {
        [SerializeField]
        private Bounds bounds = new(Vector3.zero, Vector3.one);
        public Bounds Bounds
        {
            get => bounds;
            set => bounds = value;
        }

        protected override void CheckColliders()
        {
            Count = Physics.OverlapBoxNonAlloc(transform.TransformPoint(bounds.center), bounds.extents, Colliders,
                Quaternion.identity, layerMask);
        }
    }
}
