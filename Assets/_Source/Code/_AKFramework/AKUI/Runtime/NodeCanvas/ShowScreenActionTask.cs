using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKNodeCanvas;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Source.Code._AKFramework.AKUI.Runtime.NodeCanvas
{
    [Category("AKFramework/UI")]
    [Name("Show Screen")]
    [Serializable]
    public class ShowScreenActionTask : AKActionTask
    {
        public BBParameter<AKScreen> _screen;

        private IAKUIService _uiController;

        protected override void Init(IAKContainer injectionContainer)
        {
            _uiController = injectionContainer.Resolve<IAKUIService>();
        }

        protected override string info => $"<color=green>Show</color> <color=yellow>{_screen}</color> Screen";

        protected override void OnExecute()
        {
            _uiController.ShowScreen(_screen.value);
            EndAction(true);
        }
    }
}
