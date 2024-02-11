using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("SFramework/UI")]
    [Name("Show Screen")]
    [Serializable]
    public class ShowScreenConditionTask : AKConditionTask
    {
        public BBParameter<AKScreen> _screen;

        private IAKUIService _uiListener;

        protected override void Init(DiContainer injectionContainer)
        {
            _uiListener = injectionContainer.Resolve<IAKUIService>();
        }
        
        protected override bool OnCheck()
        {
            return _uiListener.GetScreenState(_screen.value) == AKScreenState.Showing;
        }

        protected override string info => $"<color=green>Show </color><color=yellow>{_screen}</color> Screen";
    }
}