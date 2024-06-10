using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKNodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Source.Code._AKFramework.AKEvents.Runtime.NodeCanvas
{
    [Category("AKFramework/Events")]
    [Name("Broadcast Event")]
    [Serializable]
    public class BroadcastAKEventActionTask : AKActionTask
    {
        public BBParameter<AKEvent> Event;

        private IAKEventsService _eventsCommand;

        protected override void Init(IAKContainer injectionContainer)
        {
            _eventsCommand = injectionContainer.Resolve<IAKEventsService>();
        }

        protected override string info => $"<color=green>Broadcast</color> <color=yellow>{Event}</color> Event";

        protected override void OnExecute()
        {
            _eventsCommand.Broadcast(Event.value);
            EndAction(true);
        }
    }
}