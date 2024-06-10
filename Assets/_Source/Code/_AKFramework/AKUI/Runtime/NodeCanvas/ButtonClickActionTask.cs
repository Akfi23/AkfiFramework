using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Button Click")]
    [Serializable]
    public class ButtonClickActionTask : AKActionTask
    {
        public BBParameter<AKButton> Button;

        private IAKUIService _uiService;

        protected override void Init(IAKContainer container)
        {
            _uiService = container.Resolve<IAKUIService>();
        }

        protected override string info => $"<color=green>Click</color> <color=yellow>{Button}</color> Button";

        protected override void OnExecute()
        {
            _uiService.ButtonClickCallback(Button.value);
            EndAction(true);
        }
    }
}