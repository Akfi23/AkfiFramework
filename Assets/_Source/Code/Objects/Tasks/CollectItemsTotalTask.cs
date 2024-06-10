using System;
using _Client_.Scripts.TaskSystem.Objects;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects.Tasks
{
    [Serializable]
    public class CollectItemsTotalTask : Task
    {
        [SerializeField] [AKTagsGroup("Items")][GUIColor("lightgreen")][SuffixLabel(SdfIconType.Star)]
        private AKTag targetItemTag;
        [SerializeField]
        private int targetValue;
        [ReadOnly] [SerializeField][GUIColor("lightblue")][SuffixLabel(SdfIconType.ArrowUpShort)]
        private int currentValue;
        [SerializeField] private int totalCollectedItemsCount;

        public override void Init(IAKContainer container, float value)
        {
            base.Init(container, value);
            currentValue = (int)value;
        }

        public override void Start()
        {
            base.Start();
            _itemService.OnItemAdd += UpdateAction;

            if (_itemService.GetTotalCollected(targetItemTag) >= totalCollectedItemsCount)
            {
                currentValue = targetValue;
                Complete();
            }
        }

        private void UpdateAction(AKTag itemTag, int value)
        {
            if (itemTag != targetItemTag) return;
            if (_itemService.GetTotalCollected(targetItemTag) < totalCollectedItemsCount) return;
            
            if (IsComplete()) return;
            DoAction();
            if (!IsComplete()) return;
            Complete();
        }

        public override void DoAction()
        {
            currentValue++;
            base.DoAction();
        }

        public override void Complete()
        {
            _itemService.OnItemAdd -= UpdateAction;
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

        public override bool IsComplete() => currentValue >= targetValue;

        public override AKTag GetTargetTag()
        {
            return targetItemTag;
        }
    }
}