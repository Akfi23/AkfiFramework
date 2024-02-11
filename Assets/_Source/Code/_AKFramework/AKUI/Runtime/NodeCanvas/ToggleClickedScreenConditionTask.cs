using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Toggle Clicked")]
    [Serializable]
    public class ToggleClickedScreenConditionTask : AKConditionTask
    {
        public BBParameter<AKToggle> Toggle;

        private IAKUIService _uiListener;

        protected override void Init(DiContainer injectionContainer)
        {
            _uiListener = injectionContainer.Resolve<IAKUIService>();
            _uiListener.OnToggleClick += OnToggleClick;
        }

        private void OnToggleClick(AKToggle toggle, bool value)
        {
            if (Toggle.value != toggle) return;
            YieldReturn(true);
        }

        protected override bool OnCheck()
        {
            return false;
        }

        protected override string info => $"<color=green>Clicked </color><color=yellow>{Toggle}</color> Toggle";
    }
}