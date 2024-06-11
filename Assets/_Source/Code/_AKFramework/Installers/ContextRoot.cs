using _Source.Code._AKFramework.AKCore.Runtime;
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

namespace _Source.Code._AKFramework.Installers
{
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

        protected override void Setup(IAKContainer container)
        {
            foreach (var database in databases)
            {
                container.Bind(database);
            }

            Container.Bind<IAKWorldService>(new AKWorldService());
            Container.Bind<IAKScenesService>(new AKScenesService());
            Container.Bind<IAKUIService>(new AKUIService());
            Container.Bind<IAKTagsService>(new AKTagsService());
            Container.Bind<IAKEventsService>(new AKEventsService());
            Container.Bind<IAKPoolsService>(new AKPoolsService());
            Container.Bind<IInputService>(new InputService());
            Container.Bind<EcsPoolService>(new EcsPoolService());
            Container.Bind<IItemService<int>>(new ItemService());
            Container.Bind<UpgradesService>(new UpgradesService());
            Container.Bind<ISaveService>(new SaveService());
            Container.Bind<BuildingsService>(new BuildingsService());
            Container.Bind<MaterialService>(new MaterialService());
            Container.Bind<IVibrationService>(new VibrationService());
            Container.Bind<PlayerDataService>(new PlayerDataService());
            Container.Bind<SettingsService>(new SettingsService());
            Container.Bind<TaskService>(new TaskService());
            Container.Bind<VFXService>(new VFXService());
            Container.Bind<TechService>(new TechService());
        }

        protected override void Init(IAKContainer container)
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
