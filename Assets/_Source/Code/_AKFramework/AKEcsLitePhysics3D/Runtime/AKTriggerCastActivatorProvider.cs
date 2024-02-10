using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using _Source.Code._Core.View;
using Leopotam.EcsLite;
using SFramework.ECS.Lite.Physics3D.Runtime;
using UnityEngine;

namespace _Client_.Scripts.ECS.Authorings
{
    [RequireComponent(typeof(_AKTriggerCastEvent))]
    public class AKTriggerCastActivatorProvider : AKView, IAKEntitySetup
    {
        private EcsWorld _world;
        private EcsPackedEntity _packedEntity;
        private EcsPool<AKTrigger3D_Active> _activeTriggerPool;

        public void Setup(ref EcsWorld world, ref EcsPackedEntity packedEntity)
        {
            _world = world;
            _packedEntity = packedEntity;
            _activeTriggerPool = _world.GetPool<AKTrigger3D_Active>();
        }

        protected void OnEnable()
        {
            HandleTriggerActivator();
        }

        protected void OnDisable()
        {
            HandleTriggerActivator();
        }

        private void HandleTriggerActivator()
        {
            if (_packedEntity.Unpack(_world, out var entity))
            {
                if (gameObject.activeInHierarchy)
                {
                    if(_activeTriggerPool.Has(entity)) return;
                    
                    _activeTriggerPool.Add(entity);
                }
                else
                {
                    if(!_activeTriggerPool.Has(entity)) return;

                    _activeTriggerPool.Del(entity);
                }
            }
        }
    }
}
