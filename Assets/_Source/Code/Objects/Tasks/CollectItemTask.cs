using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Source.Code.Objects.Tasks
{
    [Serializable]
    public class CollectItemTask : Task
    {
        [SerializeField] [AKTagsGroup("Items")][GUIColor("lightgreen")][SuffixLabel(SdfIconType.Star)]
        private AKTag targetItemTag;
        [SerializeField]
        private int targetValue;
        [ReadOnly] [SerializeField][GUIColor("lightblue")][SuffixLabel(SdfIconType.ArrowUpShort)]
        private int currentValue;
        
        public override void Init(DiContainer container, float value)
        {
            base.Init(container, value);
            currentValue = (int)value;
        }

        public override void Start()
        {
            base.Start();
            _itemService.OnItemAdd += UpdateAction;
            // currentValue = 0;
        }

        private void UpdateAction(AKTag itemTag, int value)
        {
            if (itemTag != targetItemTag) return;

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
