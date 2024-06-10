using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKScenes.Runtime;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

#if ECS_EXIST

#endif

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    public class AKSplitPoolService : IAKPoolsService
    {
        [AKInject] 
        private readonly IAKContainer _container;
        [AKInject] 
        private readonly AKPoolsDatabase _akPoolsDatabase;
        [AKInject]
        private readonly IAKScenesService _scenesService;

        public Action<GameObject, bool> OnPoolSpawn { get; set; } = delegate {  };
        public Action<GameObject> OnPoolDespawn { get; set; } = delegate {  };
        public bool IsInitialized { get; private set; } = false;
        private bool IsPreInitialized { get; set; } = false;

        private readonly Dictionary<AKPrefab, GameObject> _akPrefabToAsset = new();
        private readonly Dictionary<AKPrefab, IObjectPool<GameObject>> _akPrefabToPool = new();
        private readonly Dictionary<GameObject, AKPrefab> _instanceToAKPrefab = new();
        private readonly Dictionary<GameObject, Component> _instanceToComponent = new();
        private readonly Dictionary<AKScene, HashSet<IObjectPool<GameObject>>> _sceneToPool = new();
        private readonly Dictionary<AKScene, Transform> _sceneParent = new();

        private Transform _poolParent;

#if ECS_EXIST

        private EcsWorld _ecsWorld;
        [AKInject]
        private IAKWorldService _worldsService;

        private HashSet<Type> _removeComponentHashSet = new();
        private readonly Dictionary<GameObject, List<IAKPoolReset>> _instanceToReset = new();
#endif

        [AKInject]
        protected async void Init()
        {
#if ECS_EXIST
            _ecsWorld = _worldsService.Default;
            _removeComponentHashSet = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsValueType && typeof(IAKPoolRemove).IsAssignableFrom(t)).ToHashSet();
#endif
            
            _poolParent = new GameObject("[~AK-POOL-PARENT~]").transform;
            
            _scenesService.OnSceneLoad += StartSceneLoad;
            _scenesService.OnSceneUnload += StartSceneUnload;
            
            foreach (var prefabsGroupContainer in _akPoolsDatabase.PrefabsGroupContainers)
            {
                foreach (var prefabContainer in prefabsGroupContainer.PrefabContainers)
                {
                    var prefabData = prefabContainer.PrefabData;
                    
                    if (prefabData == null) continue;
                    if (!prefabData.PrefabAssetReference.RuntimeKeyIsValid()) continue;
                    if(prefabData is AKSplitPrefabData) continue;
                    
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

            IsPreInitialized = true;
        }

        private void StartSceneUnload(AKScene akScene)
        {
            if (_sceneToPool.ContainsKey(akScene))
            {
                foreach (var pool in _sceneToPool[akScene])
                {
                    if (_akPrefabToPool.ContainsValue(pool))
                    {
                        var akPrefab = _akPrefabToPool.First(x => x.Value == pool).Key;
                        Addressables.ReleaseInstance(_akPrefabToAsset[akPrefab]);
                    }
                    pool.Clear();
                }
                
                _sceneToPool.Remove(akScene);
            }

            if (_sceneParent.ContainsKey(akScene))
            {
                Object.Destroy(_sceneParent[akScene].gameObject);
                _sceneParent.Remove(akScene);
            }
        }

        private async void StartSceneLoad(AKScene akScene)
        {
            IsInitialized = false;
            while (!IsPreInitialized)
            {
                await Task.Yield();
            }

            if (!_sceneParent.ContainsKey(akScene))
            {
                var newParent = new GameObject($"~{akScene._Name}~").transform;
                newParent.SetParent(_poolParent);
                newParent.localPosition = Vector3.zero;
                _sceneParent.Add(akScene, newParent);
            }
            
            foreach (var prefabsGroupContainer in _akPoolsDatabase.PrefabsGroupContainers)
            {
                foreach (var prefabContainer in prefabsGroupContainer.PrefabContainers)
                {
                    var prefabData = prefabContainer.PrefabData;
                    
                    if (prefabData == null) continue;
                    if (!prefabData.PrefabAssetReference.RuntimeKeyIsValid()) continue;
                    if(prefabData is not AKSplitPrefabData data) continue;
                    var hasScene = false;
                    var sceneId = -1;
                    for(int i = 0; i < data.PoolSceneData.Length; i++)
                    {
                        if(data.PoolSceneData[i].Scene != akScene) continue;
                        hasScene = true;
                        sceneId = i;
                        break;
                    }

                    if (!hasScene) continue;

                    GameObject gameObject;
                    if (prefabData.PrefabAssetReference.Asset == null)
                    {
                        var loadAssetAsync = prefabData.PrefabAssetReference.LoadAssetAsync<GameObject>();
                        await loadAssetAsync.Task;
                        
                        gameObject = loadAssetAsync.Result;
                    }
                    else
                    {
                        gameObject = prefabData.PrefabAssetReference.Asset as GameObject;
                    }
                    
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
                            go.transform.SetParent(_sceneParent[akScene]);
#if ECS_EXIST
                            if (AKEntityMappingService.GetEntity(in go, in _ecsWorld, out var goEntity))
                            {
                                foreach (var type in _removeComponentHashSet)
                                {
                                    var pool = _ecsWorld.GetPoolByType(type);
                                    if (pool == null) continue;
                                    if (pool.Has(goEntity))
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
                        prefabData.CollectionChecks,
                        data.PoolSceneData[sceneId].CustomSettings
                            ? data.PoolSceneData[sceneId].DefaultCapacity
                            : prefabData.DefaultCapacity,
                        data.PoolSceneData[sceneId].CustomSettings
                            ? data.PoolSceneData[sceneId].MaxPoolSize
                            : prefabData.MaxPoolSize);

                    _akPrefabToPool[akPrefab] = objectPool;
                    if (prefabData is AKSplitPrefabData splitPrefabData)
                    {
                        if (_sceneToPool.ContainsKey(akScene))
                        {
                            _sceneToPool[akScene].Add(objectPool);
                        }
                        else
                        {
                            _sceneToPool[akScene] = new HashSet<IObjectPool<GameObject>> { objectPool };
                        }
                    }

                    var initInstance = new GameObject[data.PoolSceneData[sceneId].CustomSettings ? data.PoolSceneData[sceneId].SpawnOnInitCount : prefabData.SpawnOnInitCount];
                    for (int i = 0; i < initInstance.Length; i++)
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