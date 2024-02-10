using _Source.Code._AKFramework.AKECS.Runtime;
using SFramework.ECS.Lite.Physics3D.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [RequireComponent(typeof(_AKTriggerCastEvent))]
    [DisallowMultipleComponent, AddComponentMenu("AKFramework/ECS/Components/AK Sphere Trigger Cast Events"), HideMonoScript]
    public sealed class _AKSphereTriggerCastEvent : AKComponent<AKSphereTriggerCastEvent>
    {
        public AKSphereTriggerCastEvent TargetValue
        {
            get => Value;
            set => Value = value;
        }

        private void Reset()
        {
            var value = TargetValue;
            value.center = Vector3.zero;
            value.radius = 2f;
            TargetValue = value;
        }
    }
}