using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public class ConstraintUpgradeData
    {
        // [SFTagsGroup("Upgrades")]
        [SerializeField]
        [GUIColor("yellow")]
        private AKTag _upgradeTag;

        [SerializeField] 
        private int _targetUpgradeLevelIndex;
        
        [SerializeField] 
        private int _levelIndex;
        
        [SerializeField] 
        private int _taskIndex;
        
        public AKTag UpgradeTag => _upgradeTag;
        public int TargetUpgradeLevelIndex => _targetUpgradeLevelIndex;
        public int LevelIndex => _levelIndex;
        public int TaskIndex => _taskIndex;
    }
}