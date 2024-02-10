using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._Core.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [RequireComponent(typeof(AKEntity))]
    public sealed class AKCollisionEventsProvider : AKView, IAKEntitySetup
    {
        private EcsWorld _world;
        private EcsPackedEntity _packedEntity;
        private EcsPool<AKCollision3D_Enter> _collisionEnterEventPool;
        private EcsPool<AKCollision3D_Stay> _collisionStayEventPool;
        private EcsPool<AKCollision3D_Exit> _collisionExitEventPool;

        public void Setup(ref EcsWorld world, ref EcsPackedEntity entity)
        {
            _world = world;
            _packedEntity = entity;
            _collisionEnterEventPool = world.GetPool<AKCollision3D_Enter>();
            _collisionStayEventPool = world.GetPool<AKCollision3D_Stay>();
            _collisionExitEventPool = world.GetPool<AKCollision3D_Exit>();
        }

        #region Collision Event

        private void OnCollisionEnter(Collision other)
        {
            if (!AKEntityMappingService.GetEntityPacked(other.gameObject, _world, out var otherEntity)) return;
            if (!_packedEntity.Unpack(_world, out var entity)) return;

            ref var enter = ref _collisionEnterEventPool.Has(entity)
                ? ref _collisionEnterEventPool.Get(entity)
                : ref _collisionEnterEventPool.Add(entity);

            if (!enter.Other.Contains(otherEntity))
            {
                enter.Other.Add(otherEntity);
                enter.Collisions.Add(other);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (!AKEntityMappingService.GetEntityPacked(other.gameObject, _world, out var otherEntity)) return;
            if (!_packedEntity.Unpack(_world, out var entity)) return;
            ref var stay = ref _collisionStayEventPool.Has(entity)
                ? ref _collisionStayEventPool.Get(entity)
                : ref _collisionStayEventPool.Add(entity);
            
            if (!stay.Other.Contains(otherEntity))
            {
                stay.Other.Add(otherEntity);
                stay.Collisions.Add(other);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (!AKEntityMappingService.GetEntityPacked(other.gameObject, _world, out var otherEntity)) return;
            if (!_packedEntity.Unpack(_world, out var entity)) return;
            ref var exit = ref _collisionExitEventPool.Has(entity)
                ? ref _collisionExitEventPool.Get(entity)
                : ref _collisionExitEventPool.Add(entity);
            
            if (!exit.Other.Contains(otherEntity))
            {
                exit.Other.Add(otherEntity);
                exit.Collisions.Add(other);
            }
        }

        #endregion
    }
}