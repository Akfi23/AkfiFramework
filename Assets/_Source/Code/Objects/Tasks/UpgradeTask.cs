using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Source.Code.Objects.Tasks
{
    [Serializable]
    public class UpgradeTask : Task
    {
        [SerializeField] [AKTagsGroup("Upgrades")][GUIColor("lightgreen")][SuffixLabel(SdfIconType.Star)]
        private AKTag _upgradeTag;
        [SerializeField]
        private int _targetValue;
        [ReadOnly][SerializeField][GUIColor("lightblue")][SuffixLabel(SdfIconType.ArrowUpShort)]
        private int currentValue;
        [SerializeField] [AKTagsGroup("Buildings")]
        private AKTag _targetBuildingTag;

        
        private UpgradesService _upgradesService;
        
        public override void Init(DiContainer container, float value)
        {
            base.Init(container, value);

            currentValue = (int)value;

            _upgradesService = container.Resolve<UpgradesService>();
            
            _targetValue = Mathf.Clamp(_targetValue,0,_upgradesService.GetUpgradesMaxCount(_upgradeTag));
        }

        public override void Start()
        {
            base.Start();

            _upgradesService.OnGetUpgrade += UpdateAction;
        }

        private void UpdateAction(AKTag upgradeTag)
        {
            if (_upgradeTag != upgradeTag)
                return;
            
            
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
            _upgradesService.OnGetUpgrade -= UpdateAction;

            base.Complete();
        }
        
        public override float GetTargetValue()
        {
            return Mathf.Clamp(_targetValue,0,_upgradesService.GetUpgradesMaxCount(_upgradeTag));
        }

        public override bool IsComplete() => 
            currentValue >= _targetValue;

        public override AKTag GetTargetTag()
        {
            return _targetBuildingTag;
        }

        public override float GetCurrentValue()
        {
            return currentValue;
        }
    }
}