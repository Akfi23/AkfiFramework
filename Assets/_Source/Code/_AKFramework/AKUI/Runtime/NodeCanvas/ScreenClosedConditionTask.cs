using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Screen Closed")]
    [Serializable]
    public class ScreenClosedConditionTask : AKConditionTask
    {
        public BBParameter<AKScreen> _screen;

        private IAKUIService _uiListener;

        protected override void Init(IAKContainer injectionContainer)
        {
            _uiListener = injectionContainer.Resolve<IAKUIService>();
        }

        protected override bool OnCheck()
        {
            return _uiListener.GetScreenState(_screen.value) == AKScreenState.Closed;
        }

        protected override string info => $"<color=green>Screen </color><color=yellow>{_screen}</color> Closed";
    }
}