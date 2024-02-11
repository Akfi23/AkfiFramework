using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using _Source.Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Source.Code.Objects.Tasks
{
    // [Serializable][GUIColor("white")][SuffixLabel(SdfIconType.ListStars)]
    public abstract class Task : ITask
    {
        [SerializeField] 
        private Sprite icon;
        [SerializeField] 
        private string title;
        [SerializeField]
        private string description;
        [SerializeField] 
        private bool _needAutofocusCamera;
        [SerializeField] 
        private bool _needArrowTargeting;
        [SerializeField][GUIColor("lightyellow")][SuffixLabel(SdfIconType.CashCoin)]
        protected ItemData[] rewardData;

        protected DiContainer _container;
        protected IItemService<int> _itemService;
        protected TaskService _taskService;
        // protected ISFAnalyticsService _analyticsService;
        protected TechService _techService;

        public virtual void Init(DiContainer container, float value)
        {
            _container = container;
            _itemService = container.Resolve<IItemService<int>>();
            _taskService = container.Resolve<TaskService>();
            // _analyticsService = container.Resolve<ISFAnalyticsService>();
            _techService = container.Resolve<TechService>();
        }

        public virtual void Start()
        {
            
        }

        public virtual Sprite GetIcon()
        {
            return icon;
        }

        public virtual string GetTitle()
        {
            return title;
        }

        public virtual string GetDescription()
        {
            return description;
        }

        public virtual void DoAction()
        {
            _taskService.OnTaskUpdate.Invoke();
        }

        public virtual ItemData[] GetRewardData()
        {
            return rewardData;
        }

        public virtual bool HasReward()
        {
            return rewardData != null && rewardData.Length > 0;
        }

        public virtual void GetReward()
        {
            foreach (var reward in rewardData)
            {
                _itemService.Add(reward.ItemTag, reward.Value);
                
                // _analyticsService.SendCustomAnalyticsEvent(new CurrencyGainAnalyticsEvent(reward.ItemTag._Name,"Gain On Task Reward",reward.Value,1, 
                //     _techService.DaysSinceReg,_techService.TotalPlayMinutes,_techService.TotalPlaySeconds,_techService.LaunchCount));
            }
        }

        public virtual void Complete()
        {
            _taskService.OnTaskComplete.Invoke();
        }

        public virtual bool CanComplete()
        {
            return false;
        }

        public bool NeedAutofocusCamera() => 
            _needAutofocusCamera;

        public bool NeedArrowTargeting() => 
            _needArrowTargeting;

        public abstract AKTag GetTargetTag();

        public abstract float GetCurrentValue();

        public abstract float GetTargetValue();
        public abstract bool IsComplete();
    }
}
