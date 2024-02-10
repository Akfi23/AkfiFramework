using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._Core.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [RequireComponent(typeof(AKEntity))]
    public abstract class AKTriggerCastEventsProvider : AKView, IAKEntitySetup
    {
        [SerializeField] 
        private int hitCount = 10;
        [SerializeField] 
        protected LayerMask layerMask;

        private EcsWorld _world;
        private EcsPackedEntity _packedEntity;
        private EcsPool<AKTrigger3D_Enter> _triggerEnterEventPool;
        private EcsPool<AKTrigger3D_Stay> _triggerStayEventPool;
        private EcsPool<AKTrigger3D_Exit> _triggerExitEventPool;

        protected Collider[] Colliders;
        protected int Count = 0;
        private readonly Dictionary<EcsPackedEntity, bool> _entitiesDictionary = new();
        private readonly HashSet<EcsPackedEntity> _enterHashSet = new();
        private readonly HashSet<EcsPackedEntity> _stayHashSet = new();
        private readonly HashSet<EcsPackedEntity> _exitHashSet = new();

        public virtual void Setup(ref EcsWorld world, ref EcsPackedEntity entity)
        {
            _world = world;
            _packedEntity = entity;
            _triggerEnterEventPool = world.GetPool<AKTrigger3D_Enter>();
            _triggerStayEventPool = world.GetPool<AKTrigger3D_Stay>();
            _triggerExitEventPool = world.GetPool<AKTrigger3D_Exit>();

            Colliders = new Collider[hitCount];
            _entitiesDictionary.EnsureCapacity(hitCount);
        }

        private void FixedUpdate()
        {
            if (!_packedEntity.Unpack(_world, out var entity)) return;

            ClearData();
            CheckColliders();
            CheckEvents(ref entity);
        }

        protected virtual void ClearData()
        {
            _enterHashSet.Clear();
            _stayHashSet.Clear();
            _exitHashSet.Clear();

            foreach (var key in _entitiesDictionary.Keys.ToArray())
            {
                _entitiesDictionary[key] = false;
            }
        }

        protected virtual void CheckEvents(ref int entity)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!AKEntityMappingService.GetEntityPacked(Colliders[i].gameObject, _world, out var otherEntity)) continue;
                if (!_entitiesDictionary.ContainsKey(otherEntity))
                {
                    if (!_enterHashSet.Contains(otherEntity))
                    {
                        _enterHashSet.Add(otherEntity);
                    }

                    _entitiesDictionary.Add(otherEntity, true);
                }
                else
                {
                    if (!_stayHashSet.Contains(otherEntity))
                    {
                        _stayHashSet.Add(otherEntity);
                    }

                    _entitiesDictionary[otherEntity] = true;
                }
            }
                    
            foreach (var (key,value) in _entitiesDictionary)
            {
                if (value) continue;
                if (!_exitHashSet.Contains(key))
                {
                    _exitHashSet.Add(key);
                }
            }
            
            foreach (var key in _entitiesDictionary.Keys.ToArray())
            {
                if(_entitiesDictionary[key]) continue;
                _entitiesDictionary.Remove(key);
            }

            if (_enterHashSet.Count > 0)
            {
                ref var enter = ref _triggerEnterEventPool.Has(entity)
                    ? ref _triggerEnterEventPool.Get(entity)
                    : ref _triggerEnterEventPool.Add(entity);
                enter.Other.AddRange(_enterHashSet);
            }

            if (_stayHashSet.Count > 0)
            {
                ref var stay = ref _triggerStayEventPool.Has(entity)
                    ? ref _triggerStayEventPool.Get(entity)
                    : ref _triggerStayEventPool.Add(entity);
                stay.Other.AddRange(_stayHashSet);
            }

            if (_exitHashSet.Count > 0)
            {
                ref var exit = ref _triggerExitEventPool.Has(entity)
                    ? ref _triggerExitEventPool.Get(entity)
                    : ref _triggerExitEventPool.Add(entity);
                exit.Other.AddRange(_exitHashSet);
            }
        }

        protected abstract void CheckColliders();
    }
}
