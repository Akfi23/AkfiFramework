using System;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    [Obsolete]
    [RequireComponent(typeof(AKEntity))]
    public abstract class AKProvider<T> : AKProvider where T : struct
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private T _value;

        private EcsPackedEntity _packedEntity;
        private EcsWorld _world;

        protected T Value => _value;
        protected EcsPackedEntity PackedEntity => _packedEntity;
        protected EcsWorld World => _world;

        public override void Setup(ref EcsWorld world, ref EcsPackedEntity packedEntity)
        {
            _world = world;
            _packedEntity = packedEntity;
            if (!packedEntity.Unpack(world, out var entity)) return;
            var value = _value as IEcsInit<T>;
            value?.Init(ref _value);
            world.GetPool<T>().Add(entity) = _value;
        }
        
    }
    
    [RequireComponent(typeof(AKEntity))]
    public abstract class AKProvider : MonoBehaviour, IAKProvider
    {
        public abstract void Setup(ref EcsWorld world, ref EcsPackedEntity packedEntity);
    }
}
