using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Button Clicked")]
    [Serializable]
    public class ButtonClickedScreenConditionTask : AKConditionTask
    {
        public BBParameter<AKButton> Button;

        private IAKUIService _uiListener;

        protected override void Init(IAKContainer injectionContainer)
        {
            _uiListener = injectionContainer.Resolve<IAKUIService>();
            _uiListener.OnButtonClick += OnButtonClick;
        }

        private void OnButtonClick(AKButton button)
        {
            if (Button.value != button) return;
            YieldReturn(true);
        }

        protected override bool OnCheck()
        {
            return false;
        }

        protected override string info => $"<color=green>Clicked </color><color=yellow>{Button}</color> Button";
    }
}