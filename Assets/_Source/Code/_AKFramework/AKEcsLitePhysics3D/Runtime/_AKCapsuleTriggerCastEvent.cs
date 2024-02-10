using _Source.Code._AKFramework.AKECS.Runtime;
using SFramework.ECS.Lite.Physics3D.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [RequireComponent(typeof(_AKTriggerCastEvent))]
    [DisallowMultipleComponent, AddComponentMenu("AKFramework/ECS/Components/AK Capsule Trigger Cast Events"), HideMonoScript]
    public sealed class _AKCapsuleTriggerCastEvent : AKComponent<AKCapsuleTriggerCastEvent>
    {
        public AKCapsuleTriggerCastEvent TargetValue
        {
            get => Value;
            set => Value = value;
        }

        private void Reset()
        {
            var value = TargetValue;
            value.point1 = Vector3.zero;
            value.point2 = new Vector3(0f, 4f, 0f);
            value.radius = 2f;
            TargetValue = value;
        }
    }
}