using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKScenes.Runtime.NodeCanvas
{
    [Category("AKFramework/Scenes")]
    [Name("Unload Scene")]
    [Serializable]
    public class UnloadSceneActionTask :AKActionTask
    {
        public BBParameter<AKScene> _scene;

        private IAKScenesService _scenesService;

        protected override void Init(DiContainer injectionContainer)
        {
            _scenesService = injectionContainer.Resolve<IAKScenesService>();
        }

        protected override string info => $"<color=red>Unload</color> <color=yellow>{_scene}</color> Scene";

        protected override void OnExecute()
        {
            _scenesService.UnloadScene(_scene.value, () =>
            {
                EndAction(true);
            });
        }
    }
}