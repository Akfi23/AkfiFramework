using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public class AKPhysics3DCleanSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        
        private EcsFilter _triggerEnterEventFilter;
        private EcsFilter _triggerStayEventFilter;
        private EcsFilter _triggerExitEventFilter;
        
        private EcsFilter _collisionEnterEventFilter;
        private EcsFilter _collisionStayEventFilter;
        private EcsFilter _collisionExitEventFilter;
        
        private EcsPool<AKTrigger3D_Enter> _triggerEnterEventPool;
        private EcsPool<AKTrigger3D_Stay> _triggerStayEventPool;
        private EcsPool<AKTrigger3D_Exit> _triggerExitEventPool;
        
        private EcsPool<AKCollision3D_Enter> _collisionEnterEventPool;
        private EcsPool<AKCollision3D_Stay> _collisionStayEventPool;
        private EcsPool<AKCollision3D_Exit> _collisionExitEventPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _triggerEnterEventFilter = _world.Filter<AKTrigger3D_Enter>().End();
            _triggerStayEventFilter = _world.Filter<AKTrigger3D_Stay>().End();
            _triggerExitEventFilter = _world.Filter<AKTrigger3D_Exit>().End();
            
            _triggerEnterEventPool = _world.GetPool<AKTrigger3D_Enter>();
            _triggerStayEventPool = _world.GetPool<AKTrigger3D_Stay>();
            _triggerExitEventPool = _world.GetPool<AKTrigger3D_Exit>();
            
            _collisionEnterEventFilter = _world.Filter<AKCollision3D_Enter>().End();
            _collisionStayEventFilter = _world.Filter<AKCollision3D_Stay>().End();
            _collisionExitEventFilter = _world.Filter<AKCollision3D_Exit>().End();
            
            _collisionEnterEventPool = _world.GetPool<AKCollision3D_Enter>();
            _collisionStayEventPool = _world.GetPool<AKCollision3D_Stay>();
            _collisionExitEventPool = _world.GetPool<AKCollision3D_Exit>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _triggerEnterEventFilter)
            {
                _triggerEnterEventPool.Del(entity);
            }

            foreach (var entity in _triggerStayEventFilter)
            {
                _triggerStayEventPool.Del(entity);
            }

            foreach (var entity in _triggerExitEventFilter)
            {
                _triggerExitEventPool.Del(entity);
            }
            
            foreach (var entity in _collisionEnterEventFilter)
            {
                _collisionEnterEventPool.Del(entity);
            }

            foreach (var entity in _collisionStayEventFilter)
            {
                _collisionStayEventPool.Del(entity);
            }

            foreach (var entity in _collisionExitEventFilter)
            {
                _collisionExitEventPool.Del(entity);
            }
        }
    }
}