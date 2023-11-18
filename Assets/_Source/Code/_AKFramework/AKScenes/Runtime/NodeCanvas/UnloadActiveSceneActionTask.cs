using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKScenes.Runtime.NodeCanvas
{
    [Category("AKFramework/Scenes")]
    [Name("Unload Active Scene")]
    [Serializable]
    public class UnloadActiveSceneActionTask : AKActionTask
    {
        private IAKScenesService _scenesService;

        protected override void Init(DiContainer injectionContainer)
        {
            _scenesService = injectionContainer.Resolve<IAKScenesService>();
        }

        protected override string info => $"<color=red>Unload</color> <color=yellow>ACTIVE</color> Scene";

        protected override void OnExecute()
        {
            if (_scenesService.GetActiveScene(out AKScene activeSFScene))
            {
                _scenesService.UnloadScene(activeSFScene, () => { EndAction(true); });
                return;
            }

            EndAction(true);
        }
    }
}