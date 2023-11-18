using System;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    [Serializable]
    [RequireComponent(typeof(AKEntity))]
    public abstract class AKComponent<T> : MonoBehaviour, IAKComponent where T : struct
    {
        [HideLabel] [InlineProperty] [SerializeField] [OnValueChanged("UpdateValue", true)]
        private T _value;

        private EcsPackedEntity _packedEntity;
        private EcsWorld _world;

        protected T Value
        {
            get => _value;
            set => _value = value;
        }

        protected EcsPackedEntity PackedEntity => _packedEntity;
        protected EcsWorld World => _world;

        public void Setup(ref EcsWorld world, ref EcsPackedEntity packedEntity)
        {
            _world = world;
            _packedEntity = packedEntity;
            if (!packedEntity.Unpack(world, out var entity)) return;
            var value = _value as IEcsInit<T>;
            value?.Init(ref _value);
            world.GetPool<T>().Add(entity) = _value;
        }

        #if UNITY_EDITOR
        private void UpdateValue()
        {
            if (!Application.isPlaying) return;

            if (!PackedEntity.Unpack(World, out var entity)) return;

            World.GetPool<T>().Get(entity) = _value;
        }
        #endif
    }
}