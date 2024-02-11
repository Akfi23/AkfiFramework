using System;
using System.Collections.Generic;
using _Client_.Scripts.Interfaces;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using _Source.Code.Objects;
using UnityEngine;
using Zenject;

namespace _Source.Code.Services
{
    public class UpgradesService
    {
        [Inject]
        private UpgradesDatabase _upgradesDatabase;
        [Inject]
        private ISaveService _saveService;
        [Inject] 
        private TaskService _taskService;
        
        public Action<AKTag> OnGetUpgrade = delegate { };

        private Dictionary<AKTag, UpgradeData> _upgradesMapping = new();
        private Dictionary<AKTag, UpgradeDataContainer> _upgradesContainerMapping = new();

        private Dictionary<Tuple<AKTag, int>, Tuple<int, int>> _constraintsDataMapping = new();

        [Inject]
        private void Init()
        {
            _upgradesMapping.Clear();
            _upgradesContainerMapping.Clear();
            
            _constraintsDataMapping.Clear();

            foreach (var data in _upgradesDatabase.UpgradeDatas)
            {
                _upgradesMapping.Add(data.UpgradeTag,data);
                
                _upgradesContainerMapping.Add(data.UpgradeTag,_saveService.Load(data.UpgradeTag._Name,new UpgradeDataContainer()));
            }

            foreach (var constraintData in _upgradesDatabase.ConstraintUpgradeDatas)
            {
                _constraintsDataMapping.Add(new Tuple<AKTag, int>(constraintData.UpgradeTag, constraintData.TargetUpgradeLevelIndex),
                    new Tuple<int, int>(constraintData.LevelIndex, constraintData.TaskIndex));
            }
        }

        public string GetUpgradeName(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].Name;
        }

        public Sprite GetUpgradeIcon(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].Icon;
        }

        public AKTag GetUpgradePayItemTag(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].PayItemTag;
        }

        public int GetUpgradeLevel(AKTag upgradeTag)
        {
            return _upgradesContainerMapping[upgradeTag].Level;
        }

        public void IncreaseUpgradeLevel(AKTag upgradeTag)
        {
            if(_upgradesContainerMapping[upgradeTag].Level>= GetUpgradeValuesCount(upgradeTag)) return;

            _upgradesContainerMapping[upgradeTag].Level++;
            
            _saveService.Save(upgradeTag._Name, GetDataContainer(upgradeTag));
            OnGetUpgrade?.Invoke(upgradeTag);
        }

        public bool CheckExistUpgradeLevel(AKTag upgradeTag)
        {
            return _upgradesContainerMapping[upgradeTag].Level < GetUpgradeValuesCount(upgradeTag)-1;
        }

        public int GetUpgradePrice(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].Prices[Mathf.Clamp(GetUpgradeLevel(upgradeTag),0,GetPricesCount(upgradeTag))];
        }

        public float GetUpgradeValue(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].UpgradeValues[Mathf.Clamp(GetUpgradeLevel(upgradeTag),0,GetUpgradeValuesCount(upgradeTag)-1)];
        }

        public float GetUpgradeNextValue(AKTag upgradeTag)
        {
            int index = Mathf.Clamp(GetUpgradeLevel(upgradeTag)+1, GetUpgradeLevel(upgradeTag),
                GetUpgradeValuesCount(upgradeTag) - 1);
            
            return _upgradesMapping[upgradeTag].UpgradeValues[index];
        }

        private int GetPricesCount(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].Prices.Length;
        }

        public int GetUpgradesMaxCount(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].UpgradeValues.Length;
        }

        public bool ExistUpgradeConstraints(AKTag upgradeTag, out int neededTaskIndex)
        {
            int upgradeLevel = GetUpgradeLevel(upgradeTag);
            neededTaskIndex = 0;

            if (!_constraintsDataMapping.ContainsKey(new Tuple<AKTag, int>(upgradeTag, upgradeLevel)))
                return false;


            if (_taskService.GetCurrentTaskIndex().CurrentIndex >= _constraintsDataMapping[new Tuple<AKTag, int>(upgradeTag, upgradeLevel)].Item2)
                return false;

            neededTaskIndex = _constraintsDataMapping[new Tuple<AKTag, int>(upgradeTag, upgradeLevel)].Item2;
            return true;
        }

        private UpgradeDataContainer GetDataContainer(AKTag upgradeTag)
        {
            return _upgradesContainerMapping[upgradeTag];
        }

        private int GetUpgradeValuesCount(AKTag upgradeTag)
        {
            return _upgradesMapping[upgradeTag].UpgradeValues.Length;
        }
    }
}
