using System.Linq;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKECS.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public class AKPhysics3DTriggerCastSystem : AKEcsSystem
    {
        private EcsWorld _world;
        private EcsFilter _boxFilter;
        private EcsFilter _sphereFilter;
        private EcsFilter _capsuleFilter;

        private EcsPool<TransformRef> _transformPool;
        private EcsPool<AKTriggerCastEvent> _triggerEventsPool;
        private EcsPool<AKBoxTriggerCastEvent> _boxTriggerEventsPool;
        private EcsPool<AKSphereTriggerCastEvent> _sphereTriggerEventsPool;
        private EcsPool<AKCapsuleTriggerCastEvent> _capsuleTriggerEventsPool;

        private EcsPool<AKTrigger3D_Enter> _triggerEnterEventPool;
        private EcsPool<AKTrigger3D_Stay> _triggerStayEventPool;
        private EcsPool<AKTrigger3D_Exit> _triggerExitEventPool;

        private EcsPool<AKTrigger3D_Active> _triggerActiveEventPool;

        protected override void Setup(ref IEcsSystems systems, ref IAKContainer container)
        {
            _world = systems.GetWorld();
            _boxFilter = _world.Filter<AKBoxTriggerCastEvent>().Inc<AKTriggerCastEvent>().End();
            _sphereFilter = _world.Filter<AKSphereTriggerCastEvent>().Inc<AKTriggerCastEvent>().End();
            _capsuleFilter = _world.Filter<AKCapsuleTriggerCastEvent>().Inc<AKTriggerCastEvent>().End();

            _transformPool = _world.GetPool<TransformRef>();
            _triggerEventsPool = _world.GetPool<AKTriggerCastEvent>();
            _boxTriggerEventsPool = _world.GetPool<AKBoxTriggerCastEvent>();
            _sphereTriggerEventsPool = _world.GetPool<AKSphereTriggerCastEvent>();
            _capsuleTriggerEventsPool = _world.GetPool<AKCapsuleTriggerCastEvent>();
            
            _triggerEnterEventPool = _world.GetPool<AKTrigger3D_Enter>();
            _triggerStayEventPool = _world.GetPool<AKTrigger3D_Stay>();
            _triggerExitEventPool = _world.GetPool<AKTrigger3D_Exit>();

            _triggerActiveEventPool = _world.GetPool<AKTrigger3D_Active>();
        }

        public override void Tick(ref IEcsSystems systems)
        {
            foreach (var entity in _boxFilter)
            {
                ref var triggerEvent = ref _triggerEventsPool.Get(entity);
                ref var boxTriggerEvents = ref _boxTriggerEventsPool.Get(entity);
                ref var transform = ref _transformPool.Get(entity).instance;

                ClearData(ref triggerEvent);
                
                if (_triggerActiveEventPool.Has(entity))
                {
                    triggerEvent.count = Physics.OverlapBoxNonAlloc(
                        transform.TransformPoint(boxTriggerEvents.bounds.center), boxTriggerEvents.bounds.extents,
                        triggerEvent.colliders,
                        Quaternion.identity, triggerEvent.layerMask);

                    CheckEvents(ref triggerEvent, entity);
                }
                else
                {
                    InactiveEvents(ref triggerEvent, entity);
                }
            }
            
            foreach (var entity in _sphereFilter)
            {
                ref var triggerEvent = ref _triggerEventsPool.Get(entity);
                ref var sphereTriggerEvents = ref _sphereTriggerEventsPool.Get(entity);
                ref var transform = ref _transformPool.Get(entity).instance;
                
                ClearData(ref triggerEvent);

                if (_triggerActiveEventPool.Has(entity))
                {
                    triggerEvent.count = Physics.OverlapSphereNonAlloc(
                        transform.TransformPoint(sphereTriggerEvents.center), sphereTriggerEvents.radius,
                        triggerEvent.colliders, triggerEvent.layerMask);

                    CheckEvents(ref triggerEvent, entity);
                }
                else
                {
                    InactiveEvents(ref triggerEvent, entity);
                }
            }
            
            foreach (var entity in _capsuleFilter)
            {
                ref var triggerEvent = ref _triggerEventsPool.Get(entity);
                ref var capsuleTriggerEvents = ref _capsuleTriggerEventsPool.Get(entity);
                ref var transform = ref _transformPool.Get(entity).instance;
                
                ClearData(ref triggerEvent);

                if (_triggerActiveEventPool.Has(entity))
                {
                    triggerEvent.count = Physics.OverlapCapsuleNonAlloc(
                        transform.TransformPoint(capsuleTriggerEvents.point1),
                        transform.TransformPoint(capsuleTriggerEvents.point2),
                        capsuleTriggerEvents.radius, triggerEvent.colliders, triggerEvent.layerMask);

                    CheckEvents(ref triggerEvent, entity);
                }
                else
                {
                    InactiveEvents(ref triggerEvent, entity);
                }
            }
        }

        private void ClearData(ref AKTriggerCastEvent triggerEvent)
        {
            triggerEvent.enterHashSet.Clear();
            triggerEvent.stayHashSet.Clear();
            triggerEvent.exitHashSet.Clear();

            foreach (var key in triggerEvent.entitiesDictionary.Keys.ToArray())
            {
                triggerEvent.entitiesDictionary[key] = false;
            }
        }
        
        private void CheckEvents(ref AKTriggerCastEvent triggerEvent, int entity)
        {
            for (int i = 0; i < triggerEvent.count; i++)
            {
                if (!AKEntityMappingService.GetEntityPacked(triggerEvent.colliders[i].gameObject, _world, out var otherEntity)) continue;
                if (!triggerEvent.entitiesDictionary.ContainsKey(otherEntity))
                {
                    if (!triggerEvent.enterHashSet.Contains(otherEntity))
                    {
                        triggerEvent.enterHashSet.Add(otherEntity);
                    }

                    triggerEvent.entitiesDictionary.Add(otherEntity, true);
                }
                else
                {
                    if (!triggerEvent.stayHashSet.Contains(otherEntity))
                    {
                        triggerEvent.stayHashSet.Add(otherEntity);
                    }

                    triggerEvent.entitiesDictionary[otherEntity] = true;
                }
            }
                    
            foreach (var (key,value) in triggerEvent.entitiesDictionary)
            {
                if (value) continue;
                if (!triggerEvent.exitHashSet.Contains(key))
                {
                    triggerEvent.exitHashSet.Add(key);
                }
            }
            
            foreach (var key in triggerEvent.entitiesDictionary.Keys.ToArray())
            {
                if(triggerEvent.entitiesDictionary[key]) continue;
                triggerEvent.entitiesDictionary.Remove(key);
            }

            if (triggerEvent.enterHashSet.Count > 0)
            {
                ref var enter = ref _triggerEnterEventPool.Has(entity)
                    ? ref _triggerEnterEventPool.Get(entity)
                    : ref _triggerEnterEventPool.Add(entity);
                enter.Other.AddRange(triggerEvent.enterHashSet);
            }

            if (triggerEvent.stayHashSet.Count > 0)
            {
                ref var stay = ref _triggerStayEventPool.Has(entity)
                    ? ref _triggerStayEventPool.Get(entity)
                    : ref _triggerStayEventPool.Add(entity);
                stay.Other.AddRange(triggerEvent.stayHashSet);
            }

            if (triggerEvent.exitHashSet.Count > 0)
            {
                ref var exit = ref _triggerExitEventPool.Has(entity)
                    ? ref _triggerExitEventPool.Get(entity)
                    : ref _triggerExitEventPool.Add(entity);
                exit.Other.AddRange(triggerEvent.exitHashSet);
            }
        }

        private void InactiveEvents(ref AKTriggerCastEvent triggerEvent, int entity)
        {
            if(triggerEvent.entitiesDictionary.Count <= 0) return;
            foreach (var (key,value) in triggerEvent.entitiesDictionary)
            {
                if (!triggerEvent.exitHashSet.Contains(key))
                {
                    triggerEvent.exitHashSet.Add(key);
                }
            }
                    
            if (triggerEvent.exitHashSet.Count > 0)
            {
                ref var exit = ref _triggerExitEventPool.Has(entity)
                    ? ref _triggerExitEventPool.Get(entity)
                    : ref _triggerExitEventPool.Add(entity);
                exit.Other.AddRange(triggerEvent.exitHashSet);
            }
                    
            triggerEvent.entitiesDictionary.Clear();
        }
    }
}