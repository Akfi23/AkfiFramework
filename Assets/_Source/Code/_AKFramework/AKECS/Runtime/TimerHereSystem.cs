using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public class TimerHereSystem<T> : AKEcsSystem where T : struct, IAKEcsTimer
    {
        private EcsWorld _world;
        private EcsFilter _filter;

        private EcsPool<T> _pool;

        protected override void Setup(ref IEcsSystems systems, ref DiContainer container)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<T>().End();

            _pool = _world.GetPool<T>();
        }

        public override void Tick(ref IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var timer = ref _pool.Get(entity);
                timer.Timer -= Time.deltaTime;
                
                if(timer.Timer > 0) continue;
                
                _pool.Del(entity);
            }
        }
    }
}