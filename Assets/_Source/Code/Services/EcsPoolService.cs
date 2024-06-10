using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code.ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code.Services
{
    public class EcsPoolService : IAKService
    {
        [AKInject]
        private IAKWorldService _worldsService;
        [AKInject] 
        private IAKPoolsService _poolsService;

        private EcsPool<Spawned> _spawnedPool;

        [AKInject]
        private void Init()
        {
            _poolsService.OnPoolSpawn += Spawn;
            _poolsService.OnPoolDespawn += Despawn;

            _spawnedPool = _worldsService.Default.GetPool<Spawned>();
        }

        private void Despawn(GameObject obj)
        {
            if(!AKEntityMappingService.GetEntity(obj, _worldsService.Default, out var objectEntity)) return;
            
            _spawnedPool.Del(objectEntity);
        }

        private void Spawn(GameObject obj, bool isValid)
        {
            if (!isValid) return;
            if (!AKEntityMappingService.GetEntity(obj, _worldsService.Default, out var objectEntity)) return;

            _spawnedPool.Add(objectEntity);
        }
    }
}