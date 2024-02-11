using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Toggle Click")]
    [Serializable]
    public class ToggleClickActionTask : AKActionTask
    {
        public BBParameter<AKToggle> Toggle;
        public bool Value;

        private IAKUIService _uiService;

        protected override void Init(DiContainer injectionContainer)
        {
            _uiService = injectionContainer.Resolve<IAKUIService>();
        }

        protected override string info => $"<color=green>Click</color> <color=yellow>{Toggle}</color> Toggle";

        protected override void OnExecute()
        {
            _uiService.ToggleClickCallback(Toggle.value, Value);
            EndAction(true);
        }
    }
}