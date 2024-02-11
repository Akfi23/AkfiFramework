using System;
using System.Collections.Generic;
using System.Linq;
using _Source.Code._AKFramework.AKECS.Runtime;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;

#if ECS_EXIST

#endif

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    public class AKPoolsService : IAKPoolsService
    {
        [Inject] 
        private readonly DiContainer _container;
        [Inject] 
        private readonly AKPoolsDatabase _akPoolsDatabase;

        public Action<GameObject, bool> OnPoolSpawn { get; set; } = delegate {  };
        public Action<GameObject> OnPoolDespawn { get; set; } = delegate {  };
        public bool IsInitialized { get; private set; } = false;

        private readonly Dictionary<AKPrefab, GameObject> _akPrefabToAsset = new();
        private readonly Dictionary<AKPrefab, IObjectPool<GameObject>> _akPrefabToPool = new();
        private readonly Dictionary<GameObject, AKPrefab> _instanceToAKPrefab = new();
        private readonly Dictionary<GameObject, Component> _instanceToComponent = new();

        private Transform _poolParent;
        
#if ECS_EXIST

        private EcsWorld _ecsWorld;
        [Inject]
        private IAKWorldService _worldsService;

        private HashSet<Type> _removeComponentHashSet = new();
        private readonly Dictionary<GameObject, List<IAKPoolReset>> _instanceToReset = new();
#endif

        [Inject]
        protected async void Init()
        {
#if ECS_EXIST
            _ecsWorld = _worldsService.Default;
            _removeComponentHashSet = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsValueType && typeof(IAKPoolRemove).IsAssignableFrom(t)).ToHashSet();
#endif
            
            _poolParent = new GameObject("[~AK-POOL-PARENT~]").transform;

            foreach (var prefabsGroupContainer in _akPoolsDatabase.PrefabsGroupContainers)
            {
                foreach (var prefabContainer in prefabsGroupContainer.PrefabContainers)
                {
                    var prefabData = prefabContainer.PrefabData;
                    
                    if (prefabData == null) continue;
                    if (!prefabData.PrefabAssetReference.RuntimeKeyIsValid()) continue;
                    
                    var loadAssetAsync = prefabData.PrefabAssetReference.LoadAssetAsync<GameObject>();
                    await loadAssetAsync.Task;
                    
                    var gameObject = loadAssetAsync.Result;
                    var akPrefabId = prefabContainer._Id;
                    var akPrefabName = $"{prefabsGroupContainer._Name}/{prefabContainer._Name}";
                    var akPrefab = new AKPrefab(akPrefabId, akPrefabName);
                    _akPrefabToAsset[akPrefab] = gameObject;
                    
                    ObjectPool<GameObject> objectPool = null;
                    objectPool = new ObjectPool<GameObject>(() =>
                        {
                            var _gameObject = Object.Instantiate(_akPrefabToAsset[akPrefab], _poolParent, true);
                            _container.Inject(_gameObject);
                            _instanceToAKPrefab[_gameObject] = akPrefab;
#if ECS_EXIST

                            var resetList = _gameObject.GetComponents<IAKPoolReset>().ToList();
                            if (resetList.Count > 0)
                            {
                                _instanceToReset[_gameObject] = resetList;
                            }
                            
#endif

                            return _gameObject;
                        },
                        go => { go.SetActive(true); },
                        go =>
                        {
                            go.transform.SetParent(_poolParent);
#if ECS_EXIST
                            if (AKEntityMappingService.GetEntity(in go, in _ecsWorld, out var goEntity))
                            {
                                foreach (var type in _removeComponentHashSet)
                                {
                                    var pool = _ecsWorld.GetPoolByType(type);
                                    if(pool == null) continue;
                                    if(pool.Has(goEntity))
                                        pool.Del(goEntity);
                                }

                                if (_instanceToReset.ContainsKey(go))
                                {
                                    foreach (var reset in _instanceToReset[go])
                                    {
                                        reset.Reset();
                                    }
                                }
                            }
#endif
                            go.SetActive(false);
                        },
                        go =>
                        {
                            Object.Destroy(go);
                            _instanceToAKPrefab.Remove(go);
                        },
                        prefabData.CollectionChecks, prefabData.DefaultCapacity, prefabData.MaxPoolSize);

                    _akPrefabToPool[akPrefab] = objectPool;

                    var initInstance = new GameObject[prefabData.SpawnOnInitCount];
                    for (int i = 0; i < prefabData.SpawnOnInitCount; i++)
                    {
                        Spawn(akPrefab, new AKPrefabSpawnSettings(), out var go);
                        initInstance[i] = go;
                    }

                    foreach (var go in initInstance)
                    {
                        Despawn(go);
                    }
                }
            }

            IsInitialized = true;
        }
        
        public bool Spawn(AKPrefab prefab, AKPrefabSpawnSettings settings, out GameObject gameObject)
        {
            if (!_akPrefabToPool.ContainsKey(prefab))
            {
                gameObject = null;
                OnPoolSpawn.Invoke(gameObject, false);
                return false;
            }

            var pool = _akPrefabToPool[prefab];

            gameObject = pool.Get();

            if (settings.Position.HasValue)
                gameObject.transform.position = settings.Position.Value;
            
            if (settings.Rotation.HasValue)
                gameObject.transform.rotation = settings.Rotation.Value;
            
#if ECS_EXIST

            if (AKEntityMappingService.GetEntity(in gameObject, in _ecsWorld, out var entity))
            {
                ref var transformRef = ref _ecsWorld.GetPool<TransformRef>().Get(entity);
                if (settings.Position.HasValue) transformRef.InitialPosition = settings.Position.Value;
                if (settings.Rotation.HasValue) transformRef.InitialRotation = settings.Rotation.Value;
            }
                
#endif

            gameObject.transform.SetParent(settings.Parent != null ? settings.Parent : null);
            OnPoolSpawn.Invoke(gameObject, true);
            return true;
        }

        public bool Spawn<T>(AKPrefab prefab, AKPrefabSpawnSettings settings, out T component) where T : Component
        {
            if (Spawn(prefab, settings, out var gameObject))
            {
                component = gameObject.GetComponent<T>();
                return true;
            }
            
            component = null;
            return false;
        }

        public bool Despawn(GameObject gameObject)
        {
            if (!gameObject.activeSelf) return false;
            if (!_instanceToAKPrefab.ContainsKey(gameObject)) return false;
            var akPrefab = _instanceToAKPrefab[gameObject];
            if (!_akPrefabToPool.ContainsKey(akPrefab)) return false;
            var pool = _akPrefabToPool[akPrefab];
            pool.Release(gameObject);
            OnPoolDespawn.Invoke(gameObject);
            return true;
        }

        public void DespawnAll()
        {
            var keys = _instanceToAKPrefab.Keys.ToList();
            foreach (var key in keys)
            {
                Despawn(key);
            }
        }
    }
}