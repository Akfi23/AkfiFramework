using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKEvents.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using AKFramework.Generated;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code.Objects.Tutorial.TutorialStepConditions
{
    [Serializable]
    public class StoppedTimelineCondition : ITutorialStepCondition
    {
        // [SFTagsGroup("Timelines")]
        [SerializeField] 
        private AKTag _timelineTag;
        
        
        private bool _init = false;
        private bool _subscribed = false;
        private bool _timelineStopped = false;

        private IAKEventsService _eventsService;

        public string GetConditionName() => $"Stop {_timelineTag}";

        public void Init(ref EcsWorld world, ref IAKContainer container)
        {
            _eventsService = container.Resolve<IAKEventsService>();

            if (!_subscribed)
            {
                // _eventsService.AddListener<AKTag>(AKEvents.Game__Timeline_Stopped, OnTimelineStopped);

                _subscribed = true;
            }

            _init = true;
        }

        public bool CheckCondition(ref EcsWorld world, ref IAKContainer container)
        {
            if (!_init) 
                Init(ref world, ref container);

            return _timelineStopped;
        }

        private void OnTimelineStopped(AKEvent sfevent, AKTag timelineTag)
        {
            if (timelineTag != _timelineTag)
                return;


            _timelineStopped = true;
        }

        public void Reset()
        {
            if (_subscribed && _eventsService != null)
            {
                // _eventsService.RemoveListener<AKTag>(AKEvents.Game__Timeline_Stopped, OnTimelineStopped);
                _subscribed = false;
            }
            _eventsService = null;
            _init = false;
            _timelineStopped = false;
        }
    }
}