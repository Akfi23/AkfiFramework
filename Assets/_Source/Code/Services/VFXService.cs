using System.Collections.Generic;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Source.Code.Services
{
    public class VFXService
    {
        [Inject] 
        private VFXDatabase _database;
        [Inject]
        private IAKPoolsService _poolsService;

        private readonly Dictionary<AKTag, VFXData> _vfxMapping = new();

        private AKPrefabSpawnSettings _settings = new();

        [Inject]
        private async void Init()
        {
            await UniTask.WaitUntil(() => _poolsService.IsInitialized);
            
            foreach (var data in _database.VFXData)
            {
                _vfxMapping.Add(data.VFXTag, data);
            }
        }

        public bool PlayVFX(AKTag tag, Vector3 position, Quaternion rotation, out GameObject vfxObject)
        {
            if(GetData(tag) == null)
            {
                vfxObject = null;
                return false;
            }

            _settings.Position = position;
            _settings.Rotation = rotation;
            _settings.Parent = null;

            return _poolsService.Spawn(GetData(tag).VFXPrefab, _settings, out vfxObject);
        }

        private VFXData GetData(AKTag tag)
        {
            return _vfxMapping.ContainsKey(tag) ? _vfxMapping[tag] : null;
        }
    }
}
