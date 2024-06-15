using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public class AKWorldService : IAKWorldService
    {
        private readonly Dictionary<Guid, EcsWorld> _ecsWorlds = new Dictionary<Guid, EcsWorld>();
        private readonly Dictionary<Guid, bool> _ecsWorldsStates = new Dictionary<Guid, bool>();

        public AKWorldService()
        {
            Default = new EcsWorld();
            _ecsWorlds[Guid.Empty] = Default;
            _ecsWorldsStates[Guid.Empty] = true;
        
            AKDebug.Log("<color=green>ECS WORLD</color> <color=white>Created</color>");
        }

        public EcsWorld Default { get; private set; }

        public EcsWorld GetWorld(Guid guid)
        {
            return _ecsWorlds.ContainsKey(guid) ? _ecsWorlds[guid] : _ecsWorlds[Guid.Empty];
        }
        
        public EcsWorld CreateWorld(out Guid guid, bool started = true)
        {
            var ecsWorld = new EcsWorld();
            guid = new Guid();
            _ecsWorlds[guid] = ecsWorld;
            _ecsWorldsStates[guid] = started;
        
            AKDebug.Log("<color=green>ECS WORLD</color> <color=white>Created</color>");

            return ecsWorld;
        }
        
        public void DestroyWorld(Guid guid)
        {
            if (guid == Guid.Empty) return;
            if (!_ecsWorldsStates.ContainsKey(guid)) return;
            _ecsWorlds[guid].Destroy();
            _ecsWorlds.Remove(guid);
            _ecsWorldsStates.Remove(guid);
        
            AKDebug.Log("<color=red>ECS WORLD</color> Destroyed");
        }

        public void ResetDefaultWorld()
        {
            Default.Destroy();
            Default = new EcsWorld();
        
            AKDebug.Log("<color=red>ECS WORLD</color> Reseted");
        }
    }
}
