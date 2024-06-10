using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKNodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Source.Code._AKFramework.AKScenes.Runtime.NodeCanvas
{
    [Category("AKFramework/Scenes")]
    [Name("Loaded Scene")]
    [Serializable]
    public class LoadedSceneConditionTask : AKConditionTask
    {
        public BBParameter<AKScene> Scene = new BBParameter<AKScene>();

        private IAKScenesService _scenesService;

        protected override void Init(IAKContainer injectionContainer)
        {
            _scenesService = injectionContainer.Resolve<IAKScenesService>();

            if (Scene.value.IsNone)
            {
                _scenesService.OnSceneLoaded += scene => { YieldReturn(true); };
            }
        }

        protected override string info =>
            $"Loaded {Scene} Scene";

        protected override bool OnCheck()
        {
            if (Scene.value.IsNone) return false;
            return _scenesService.IsLoaded(Scene.value);
        }
    }
}