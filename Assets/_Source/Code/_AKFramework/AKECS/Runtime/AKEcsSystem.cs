using Leopotam.EcsLite;
using Zenject;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public abstract class AKEcsSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            var container = systems.GetShared<DiContainer>();
            container.LazyInject(this);
            Setup(ref systems, ref container);
        }

        public void Run(IEcsSystems systems)
        {
            Tick(ref systems);
        }

        protected virtual void Setup(ref IEcsSystems systems, ref DiContainer container)
        {
        }

        public virtual void Tick(ref IEcsSystems systems)
        {
        }
    }
}
