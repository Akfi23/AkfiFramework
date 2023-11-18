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
    public class LoadSceneConditionTask : AKConditionTask
    {
        public BBParameter<AKScene> Scene;

        protected override void Init(DiContainer injectionContainer)
        {
            injectionContainer.Resolve<IAKScenesService>().OnSceneLoad += scene =>
            {
                if (Scene.value.IsNone || Scene.value == scene)
                {
                    YieldReturn(true);
                }
            };
        }

        protected override string info =>
            $"Load {Scene} Scene";

        protected override bool OnCheck()
        {
            return false;
        }
    }
}