using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKScenes.Runtime.NodeCanvas
{
    [Category("AKFramework/Scenes")]
    [Name("Load Scene")]
    [Serializable]
    public class LoadSceneActionTask : AKActionTask
    {
        public BBParameter<AKScene> _scene;

        public bool SetActive;

        private IAKScenesService _scenesService;

        protected override void Init(DiContainer injectionContainer)
        {
            _scenesService = injectionContainer.Resolve<IAKScenesService>();
        }

        protected override string info => $"<color=green>Load</color> <color=yellow>{_scene}</color> Scene";

        protected override void OnExecute()
        {
            _scenesService.LoadScene(_scene.value, SetActive, instance => { EndAction(true); });
        }
    }
}