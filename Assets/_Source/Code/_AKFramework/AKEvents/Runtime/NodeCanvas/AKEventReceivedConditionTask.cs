using System;
using _Source.Code._AKFramework.AKNodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Zenject;

namespace _Source.Code._AKFramework.AKEvents.Runtime.NodeCanvas
{
    [Category("AKFramework/Events")]
    [Name("Event Received")]
    [Serializable]
    public class AKEventReceivedConditionTask : AKConditionTask
    {
        public BBParameter<AKEvent> Event;

        private IAKEventsService _eventsListener;

        protected override void Init(DiContainer injectionContainer)
        {
            _eventsListener = injectionContainer.Resolve<IAKEventsService>();
            _eventsListener.AddListener(Event.value, Handler);
        }

        private void Handler(AKEvent sfevent)
        {
            YieldReturn(true);
        }

        protected override bool OnCheck()
        {
            return false;
        }

        protected override string info => $"<color=green>Received </color><color=yellow>{Event}</color> Event";
    }
}