using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKECS.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public class TimerHereCallbackSystem<T, TK> : AKEcsSystem where T : struct, IAKEcsTimer where TK : struct
    {
        private EcsWorld _world;
        private EcsFilter _filter;

        private EcsPool<T> _pool;
        private EcsPool<TK> _callbackPool;

        protected override void Setup(ref IEcsSystems systems, ref IAKContainer container)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<T>().End();

            _pool = _world.GetPool<T>();
            _callbackPool = _world.GetPool<TK>();
        }

        public override void Tick(ref IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var timer = ref _pool.Get(entity);
                timer.Timer -= Time.deltaTime;
                
                if(timer.Timer > 0) continue;

                _callbackPool.Add(entity);
                _pool.Del(entity);
            }
        }
    }
}