using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Source.Code.Objects.Tasks
{
    [Serializable]
    public class PayBuildingTask : Task
    {
        [SerializeField] [AKTagsGroup("Buildings")][GUIColor("lightgreen")][SuffixLabel(SdfIconType.Star)]
        private AKTag targetBuildingTag;
        [SerializeField] 
        private int targetValue;
        [ReadOnly] [SerializeField][GUIColor("lightblue")][SuffixLabel(SdfIconType.ArrowUpShort)]
        private int currentValue;

        private BuildingsService _buildingsService;

        public override void Init(DiContainer container, float value)
        {
            base.Init(container, value);
            currentValue = (int)value;
            _buildingsService = container.Resolve<BuildingsService>();
        }

        public override void Start()
        {
            base.Start();
            _buildingsService.OnPay += UpdateAction;
            if (_buildingsService.IsUnlock(targetBuildingTag))
            {
                currentValue = targetValue;
                Complete();
            }
        }

        private void UpdateAction(AKTag buildingTag,AKTag payItemTag, int count)
        {
            if(buildingTag != targetBuildingTag) return;
            
            if (_buildingsService.IsUnlock(buildingTag)) return;
            
            if(IsComplete()) return;
            currentValue += count;
            DoAction();
            if (!IsComplete()) return;
            Complete();
        }

        public override void Complete()
        {
            _buildingsService.OnPay -= UpdateAction;
            base.Complete();
        }

        public override float GetCurrentValue()
        {
            return currentValue;
        }

        public override float GetTargetValue()
        {
            return targetValue;
        }

        public override bool IsComplete()
        {
            return currentValue >= targetValue;
        }

        public override AKTag GetTargetTag()
        {
            return targetBuildingTag;
        }
    }
}