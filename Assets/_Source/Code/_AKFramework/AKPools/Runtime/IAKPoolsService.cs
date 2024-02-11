using System;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    
    public interface IAKPoolsService : IAKService
    {
        Action<GameObject, bool> OnPoolSpawn { get; set; }
        Action<GameObject> OnPoolDespawn { get; set; }
        bool IsInitialized { get; }
        bool Spawn(AKPrefab prefab, AKPrefabSpawnSettings settings, out GameObject gameObject);
        bool Spawn<T>(AKPrefab prefab, AKPrefabSpawnSettings settings, out T component) where T : Component;
        bool Despawn(GameObject gameObject);
        void DespawnAll();
    }
}