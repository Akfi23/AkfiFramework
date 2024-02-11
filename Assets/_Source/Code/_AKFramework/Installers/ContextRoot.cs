using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKEvents.Runtime;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKScenes.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using _Source.Code._Core.Installers;
using _Source.Code.ECS.Systems;
using _Source.Code.Interfaces;
using _Source.Code.Services;
using Leopotam.EcsLite;
using NodeCanvas.Framework;
using UnityEngine;
using Zenject;

namespace _Source.Code._AKFramework.Installers
{
    [RequireComponent(typeof(SceneContext))]
    public sealed class ContextRoot : AKContextRoot
    {
        [SerializeField]
        private AKDatabase[] databases;
    
        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _updateSystems;
        private EcsSystems _lateUpdateSystems;
    
        protected override void PreInit()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            AKDebug.Log("Context Root Pre Init");
            AKDebug.Log($"Target Framerate: {Application.targetFrameRate}");
        }

        public override void InstallBindings()
        {
            foreach (var database in databases)
            {
                Container.Bind(database.GetType()).FromInstance(database);
            }
        
            Container.Bind<IAKWorldService>().To<AKWorldService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IAKScenesService>().To<AKScenesService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IAKUIService>().To<AKUIService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IAKTagsService>().To<AKTagsService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IAKEventsService>().To<AKEventsService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IAKPoolsService>().To<AKPoolsService>().FromNew().AsSingle().NonLazy();
            Container.Bind<EcsPoolService>().To<EcsPoolService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IItemService<int>>().To<ItemService>().FromNew().AsSingle().NonLazy();
            Container.Bind<UpgradesService>().To<UpgradesService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveService>().To<SaveService>().FromNew().AsSingle().NonLazy();
            Container.Bind<BuildingsService>().To<BuildingsService>().FromNew().AsSingle().NonLazy();
            Container.Bind<MaterialService>().To<MaterialService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IVibrationService>().To<VibrationService>().FromNew().AsSingle().NonLazy();
            Container.Bind<PlayerDataService>().To<PlayerDataService>().FromNew().AsSingle().NonLazy();
            Container.Bind<SettingsService>().To<SettingsService>().FromNew().AsSingle().NonLazy();
            Container.Bind<TaskService>().To<TaskService>().FromNew().AsSingle().NonLazy();
            Container.Bind<VFXService>().To<VFXService>().FromNew().AsSingle().NonLazy();

            AKDebug.Log("<color=yellow>Bindings Installed</color>");
        }

        protected override void Init(DiContainer container)
        {
            var worldsService = container.Resolve<IAKWorldService>();

            _fixedUpdateSystems = new EcsSystems(worldsService.Default, container);
            _updateSystems = new EcsSystems(worldsService.Default, container);
            _lateUpdateSystems = new EcsSystems(worldsService.Default, container);

            if (AKDebug.IsDebug)
            {
#if UNITY_EDITOR
                _fixedUpdateSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
            }
            
            _fixedUpdateSystems
                .Init();
        
            _updateSystems
                .Add(new RotatePlayerSystem())
                
                .Init();
        
            _lateUpdateSystems
                .Init();
            
            AKDebug.Log("<color=yellow>ECS Inited</color>");
        }
    
        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }
    
        private void Update()
        {
            _updateSystems?.Run();
        }
    
        private void LateUpdate()
        {
            _lateUpdateSystems?.Run();
        }
    
        private void OnDestroy()
        {
            if (_fixedUpdateSystems != null)
            {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }

            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }

            if (_lateUpdateSystems != null)
            {
                _lateUpdateSystems.Destroy();
                _lateUpdateSystems = null;
            }
        }
    }
}
