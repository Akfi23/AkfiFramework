using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKScenes.Runtime.NodeCanvas
{
    [Category("AKFramework/Scenes")]
    [Name("Reload Active Scene")]
    [Serializable]
    public class ReloadSceneActionTask : AKActionTask
    {
        private IAKScenesService _scenesService;

        protected override void Init(DiContainer injectionContainer)
        {
            _scenesService = injectionContainer.Resolve<IAKScenesService>();
        }

        protected override string info => $"<color=green>Reload</color> <color=yellow>Active</color> Scene";

        protected override void OnExecute()
        {
            _scenesService.GetActiveScene(out AKScene sfScene);
            
            _scenesService.ReloadScene(sfScene, () => { }, instance =>
            {
                EndAction(true);
            });
        }
    }
}