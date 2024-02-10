using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SFramework.ECS.Lite.Physics3D.Runtime
{
    [DisallowMultipleComponent, AddComponentMenu("AKFramework/ECS/Components/AK Trigger Cast Events"), HideMonoScript]
    public sealed class _AKTriggerCastEvent : AKComponent<AKTriggerCastEvent>
    {
        public AKTriggerCastEvent TargetValue
        {
            get => Value;
            set => Value = value;
        }

        private void Reset()
        {
            var value = TargetValue;
            value.hitCount = 10;
            TargetValue = value;
        }
    }
}