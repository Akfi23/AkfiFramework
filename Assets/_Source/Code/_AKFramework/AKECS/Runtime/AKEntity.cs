using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._Core.View;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    [HideMonoScript]
    [AddComponentMenu("AKFramework/ECS/AKEntity")]
    [DisallowMultipleComponent]
    public class AKEntity : AKView, IAKEntity
    {
        public EcsWorld World => _world;
        public EcsPackedEntity EcsPackedEntity => _ecsPackedEntity;

        [AKInject]
        private IAKWorldService _worldsService;

        [SerializeField] private bool _mapChildren;

        private EcsPackedEntity _ecsPackedEntity;
        private EcsWorld _world;
        private bool _injected;

        protected override void Init()
        {
            if (_injected) return;

            _world = _worldsService.Default;

            var _entity = _world.NewEntity();
            _ecsPackedEntity = _world.PackEntity(_entity);

            AKEntityMappingService.AddMapping(gameObject, ref _ecsPackedEntity);

            if (_mapChildren)
            {
                foreach (var childTransform in GetComponentsInChildren<Transform>())
                {
                    if (childTransform.gameObject == gameObject) continue;
                    AKEntityMappingService.AddMapping(childTransform.gameObject, ref _ecsPackedEntity);
                }
            }

            _world.GetPool<GameObjectRef>().Add(_entity) = new GameObjectRef
            {
                instance = gameObject
            };
            
            var _transform = transform;
            
            _world.GetPool<TransformRef>().Add(_entity) = new TransformRef
            {
                instance = _transform,
                InitialPosition = _transform.position,
                InitialRotation = _transform.rotation,
                InitialScale = _transform.localScale
            };

            foreach (var entitySetup in GetComponents<IAKEntitySetup>())
            {
                entitySetup.Setup(ref _world, ref _ecsPackedEntity);
            }

            _injected = true;
        }

        private void OnDestroy()
        {
            AKEntityMappingService.RemoveMapping(gameObject);

            foreach (var childTransform in GetComponentsInChildren<Transform>())
            {
                if (childTransform.gameObject == gameObject) continue;
                AKEntityMappingService.RemoveMapping(childTransform.gameObject);
            }

            if (_ecsPackedEntity.Unpack(_world, out var _entity))
                _world.DelEntity(_entity);
        }

        #if UNITY_EDITOR

        public static Action<int> OnPingEntity = delegate { };

        private const string ENTITY_NAME_FORMAT = "X8";
        private GameObject _gameObject;
        private bool IsPlay => Application.isPlaying;

        [Button]
        [ShowIf("IsPlay")]
        private void PingEntity()
        {
            if (_gameObject == null)
            {
                if (_ecsPackedEntity.EqualsTo(default)) return;
                if (!_ecsPackedEntity.Unpack(_world, out var _entity)) return;

                var gameObjects = FindObjectsOfType<GameObject>();
                foreach (var debugView in gameObjects)
                {
                    if (!debugView.gameObject.name.Contains(_entity.ToString(ENTITY_NAME_FORMAT))) continue;
                    _gameObject = debugView.gameObject;
                    break;
                }
            }

            Selection.activeObject = _gameObject;
        }

        [Button]
        [ShowIf("IsPlay")]
        private void OpenEntityWindow()
        {
            if (!EcsPackedEntity.Unpack(_world, out var entity)) return;
            OnPingEntity.Invoke(entity);
        }

        #endif
    }
}
