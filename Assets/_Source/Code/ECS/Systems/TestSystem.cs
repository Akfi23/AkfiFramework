using Leopotam.EcsLite;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKECS.Runtime;

namespace _Source.Code.ECS.Systems
{
    public class TestSystem : AKEcsSystem
    {        
        private EcsWorld _world;
        private EcsFilter _filter;
        
        //ECSPool here
        
        //Service here
        
        protected override void Setup(ref IEcsSystems systems, ref IAKContainer container)
        {
            _world = systems.GetWorld();
            
            
        }
        
        public override void Tick(ref IEcsSystems systems)
        {
            foreach(var entity in _filter)
            {
                
            }
        }
    }
}