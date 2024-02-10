using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._Core.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [RequireComponent(typeof(AKEntity))]
    public sealed class AKTriggerEventsProvider : AKView, IAKEntitySetup
    {
        private EcsWorld _world;
        private EcsPackedEntity _packedEntity;
        private EcsPool<AKTrigger3D_Enter> _triggerEnterEventPool;
        private EcsPool<AKTrigger3D_Stay> _triggerStayEventPool;
        private EcsPool<AKTrigger3D_Exit> _triggerExitEventPool;

        public void Setup(ref EcsWorld world, ref EcsPackedEntity entity)
        {
            _world = world;
            _packedEntity = entity;
            _triggerEnterEventPool = world.GetPool<AKTrigger3D_Enter>();
            _triggerStayEventPool = world.GetPool<AKTrigger3D_Stay>();
            _triggerExitEventPool = world.GetPool<AKTrigger3D_Exit>();
        }

        #region Trigger Event

        private void OnTriggerEnter(Collider other)
        {
            if (!AKEntityMappingService.GetEntityPacked(other.gameObject, _world, out var otherEntity)) return;
            if (!_packedEntity.Unpack(_world, out var entity)) return;

            ref var enter = ref _triggerEnterEventPool.Has(entity)
                ? ref _triggerEnterEventPool.Get(entity)
                : ref _triggerEnterEventPool.Add(entity);

            if (!enter.Other.Contains(otherEntity))
            {
                enter.Other.Add(otherEntity);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!AKEntityMappingService.GetEntityPacked(other.gameObject, _world, out var otherEntity)) return;
            if (!_packedEntity.Unpack(_world, out var entity)) return;
            ref var stay = ref _triggerStayEventPool.Has(entity)
                ? ref _triggerStayEventPool.Get(entity)
                : ref _triggerStayEventPool.Add(entity);
            if (!stay.Other.Contains(otherEntity))
            {
                stay.Other.Add(otherEntity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!AKEntityMappingService.GetEntityPacked(other.gameObject, _world, out var otherEntity)) return;
            if (!_packedEntity.Unpack(_world, out var entity)) return;
            ref var exit = ref _triggerExitEventPool.Has(entity)
                ? ref _triggerExitEventPool.Get(entity)
                : ref _triggerExitEventPool.Add(entity);
            if (!exit.Other.Contains(otherEntity))
            {
                exit.Other.Add(otherEntity);
            }
        }

        #endregion
    }
}