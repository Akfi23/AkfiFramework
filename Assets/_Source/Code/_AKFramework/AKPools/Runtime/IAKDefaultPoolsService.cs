using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    public class IAKDefaultPoolsService : IAKPoolsService
    {
        [Inject]
        private readonly DiContainer _container;

        [Inject]
        private readonly AKPoolsDatabase _akPoolsDatabase;

        public Action<GameObject, bool> OnPoolSpawn { get; set; } = delegate {  };
        public Action<GameObject> OnPoolDespawn { get; set; } = delegate {  };
        public bool IsInitialized { get; private set; } = false;

        private readonly Dictionary<AKPrefab, GameObject> _sfPrefabToAsset = new();
        private readonly Dictionary<AKPrefab, IObjectPool<GameObject>> _sfPrefabToPool = new();
        private readonly Dictionary<GameObject, AKPrefab> _instanceToSFPrefab = new();
        private readonly Dictionary<GameObject, Component> _instanceToComponent = new();
        
        [Inject]
        protected async void Init()
        {
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
                    var sfPrefabId = prefabContainer._Id;
                    var sfPrefabName = $"{prefabsGroupContainer._Name}/{prefabContainer._Name}";
                    var sfPrefab = new AKPrefab(sfPrefabId, sfPrefabName);
                    _sfPrefabToAsset[sfPrefab] = gameObject;
                    ObjectPool<GameObject> objectPool = null;
                    objectPool = new ObjectPool<GameObject>(() =>
                        {
                            var _gameObject = Object.Instantiate(_sfPrefabToAsset[sfPrefab]);
                            _container.Inject(_gameObject);
                            _instanceToSFPrefab[_gameObject] = sfPrefab;
                            return _gameObject;
                        },
                        go => { go.SetActive(true); },
                        go => { go.SetActive(false); },
                        go =>
                        {
                            Object.Destroy(go);
                            _instanceToSFPrefab.Remove(go);
                        },
                        prefabData.CollectionChecks, prefabData.DefaultCapacity, prefabData.MaxPoolSize);

                    _sfPrefabToPool[sfPrefab] = objectPool;
                }
            }

            IsInitialized = true;
        }

        public bool Spawn(AKPrefab prefab, AKPrefabSpawnSettings settings, out GameObject gameObject)
        {
            if (!_sfPrefabToPool.ContainsKey(prefab))
            {
                gameObject = null;
                OnPoolSpawn.Invoke(gameObject, false);
                return false;
            }

            var pool = _sfPrefabToPool[prefab];
            
            gameObject = pool.Get();

            if (settings.Position.HasValue)
                gameObject.transform.position = settings.Position.Value;

            if (settings.Rotation.HasValue)
                gameObject.transform.rotation = settings.Rotation.Value;

            if (settings.Parent != null)
                gameObject.transform.SetParent(settings.Parent);

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
            if (!_instanceToSFPrefab.ContainsKey(gameObject)) return false;
            var sfPrefab = _instanceToSFPrefab[gameObject];
            if (!_sfPrefabToPool.ContainsKey(sfPrefab)) return false;
            var pool = _sfPrefabToPool[sfPrefab];
            pool.Release(gameObject);
            OnPoolDespawn.Invoke(gameObject);
            return true;
        }

        public void DespawnAll()
        {
            foreach (var asset in _instanceToSFPrefab)
            {
                Despawn(asset.Key);
            }
        }
    }
}