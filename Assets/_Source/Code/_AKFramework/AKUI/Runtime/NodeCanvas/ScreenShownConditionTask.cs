using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Screen Shown")]
    [Serializable]
    public class ScreenShownConditionTask : AKConditionTask
    {
        public BBParameter<AKScreen> _screen;

        private IAKUIService _uiListener;

        protected override void Init(DiContainer injectionContainer)
        {
            _uiListener = injectionContainer.Resolve<IAKUIService>();
        }

        protected override bool OnCheck()
        {
            return _uiListener.GetScreenState(_screen.value) == AKScreenState.Shown;
        }

        protected override string info => $"<color=green>Screen </color><color=yellow>{_screen}</color> Shown";
    }
}