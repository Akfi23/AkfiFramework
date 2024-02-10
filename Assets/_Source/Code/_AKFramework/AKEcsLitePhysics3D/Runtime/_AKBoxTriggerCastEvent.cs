using _Source.Code._AKFramework.AKECS.Runtime;
using SFramework.ECS.Lite.Physics3D.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [RequireComponent(typeof(_AKTriggerCastEvent))]
    [DisallowMultipleComponent, AddComponentMenu("AKFramework/ECS/Components/AK Box Trigger Cast Events"), HideMonoScript]
    public sealed class _AKBoxTriggerCastEvent : AKComponent<AKBoxTriggerCastEvent>
    {
        public AKBoxTriggerCastEvent TargetValue
        {
            get => Value;
            set => Value = value;
        }

        private void Reset()
        {
            var value = TargetValue;
            value.bounds.extents = Vector3.one * 0.5f;
            TargetValue = value;
        }
    }
}