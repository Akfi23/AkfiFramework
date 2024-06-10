using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.ECS.Components;
using AKFramework.Generated;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code.ECS.Systems
{
    public class RotatePlayerSystem : AKEcsSystem
    {        
        private EcsWorld _world;
        private EcsFilter _filter;

        private EcsPool<AKTagRef> _tagPool;
        private EcsPool<MovementSpeedRef> _speedPool;
        private EcsPool<TransformRef> _transformPool;

        protected override void Setup(ref IEcsSystems systems, ref IAKContainer container)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Player>().Inc<MovementSpeedRef>().Inc<AKTagRef>().End();
            
            _tagPool = _world.GetPool<AKTagRef>();
            _speedPool = _world.GetPool<MovementSpeedRef>();
            _transformPool = _world.GetPool<TransformRef>();
        }
        
        public override void Tick(ref IEcsSystems systems)
        {
            foreach(var entity in _filter)
            {
                ref var speed = ref _speedPool.Get(entity).value;
                ref var trasform = ref _transformPool.Get(entity).instance;
                ref var tag = ref _tagPool.Get(entity).tags;
                
                if(tag[0] != AKTags.Player__Player) continue;
                
                trasform.Rotate(Vector3.one * speed * Time.deltaTime);
                
                // _testService.DebugStrings();
            }
        }
    }
}