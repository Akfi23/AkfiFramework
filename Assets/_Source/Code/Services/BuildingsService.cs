using System;
using System.Collections.Generic;
using _Client_.Scripts.Interfaces;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using _Source.Code.Objects;
using Sirenix.Utilities;

namespace _Source.Code.Services
{
    public class BuildingsService : IAKService
    {
        [AKInject] 
        private BuildingsDatabase _buildingDatabase;
        [AKInject]
        private ISaveService _saveService;

        public Action<AKTag,AKTag, int> OnPay = delegate { };
        public Action<AKTag> OnUnlock = delegate { };
        public Action<AKTag> OnBuilt = delegate { };

        private readonly Dictionary<AKTag, BuildingData> _buildingDataMapping = new();
        private readonly Dictionary<AKTag, BuildingDataContainer> _buildingDataContainerMapping = new();

        public bool IsInitialized { get; private set; } = false;

        [AKInject]
        private void Init()
        {
            LoadService();

            IsInitialized = true;
        }

        public void LoadService()
        {
            _buildingDataMapping.Clear();
            _buildingDataContainerMapping.Clear();

            foreach (var data in _buildingDatabase.BuildingsData)
            {
                _buildingDataMapping.Add(data.BuildingTag, data);
                
                _buildingDataContainerMapping.Add(data.BuildingTag, _saveService.Load(data.BuildingTag._Name, new BuildingDataContainer(data.IsUnlock,data.IsBuilt)));

                if (_buildingDataContainerMapping[data.BuildingTag].Paid == null)
                {
                    _buildingDataContainerMapping[data.BuildingTag].Paid = new ItemData[data.Receipt.Length];
                    for(int i = 0; i < _buildingDataContainerMapping[data.BuildingTag].Paid.Length; i++)
                    {
                        _buildingDataContainerMapping[data.BuildingTag].Paid[i] =
                            new ItemData(data.Receipt[i].ItemTag, 0);
                    }

                    _buildingDataContainerMapping[data.BuildingTag].StartBuildTime = 0;
                }

                foreach (var paid in _buildingDataContainerMapping[data.BuildingTag].Paid)
                {
                    _buildingDataContainerMapping[data.BuildingTag].PaidMapping.Add(paid.ItemTag, paid);
                }
            }
        }

        public void ResetAreasProgress()
        {
            foreach (var data in _buildingDatabase.BuildingsData)
            {
                GetDataContainer(data.BuildingTag).IsUnlock = false;
                GetDataContainer(data.BuildingTag).PaidMapping.ForEach(x => x.Value.Value = 0);
                _saveService.Save(data.BuildingTag._Name, GetDataContainer(data.BuildingTag));
            }
        }
        
        public bool IsUnlock(AKTag buildingTag)
        {
            return GetDataContainer(buildingTag).IsUnlock;
        }

        public bool IsBuilt(AKTag buildingTag)
        {
            return GetDataContainer(buildingTag).IsBuilt;
        }

        public void SetUnlock(AKTag buildingTag, bool value)
        {
            GetDataContainer(buildingTag).IsUnlock = value;
            
            _saveService.Save(buildingTag._Name, GetDataContainer(buildingTag));
            if (value) OnUnlock.Invoke(buildingTag);
        }
        
        public void SetBuilt(AKTag buildingTag, bool value)
        {
            GetDataContainer(buildingTag).IsBuilt = value;
            
            _saveService.Save(buildingTag._Name, GetDataContainer(buildingTag));
            if (value) OnBuilt.Invoke(buildingTag);
        }

        public int GetUnlockCount()
        {
            var count = 0;
            foreach (var area in _buildingDataContainerMapping)
            {
                if (IsUnlock(area.Key)) count++;
            }
            
            return count;
        }

        public void Pay(AKTag buildingTag,AKTag itemTag, int value)
        {
            if(IsUnlock(buildingTag)) return;

            GetDataContainer(buildingTag).PaidMapping[itemTag].Value += value;
            _saveService.Save(buildingTag._Name, GetDataContainer(buildingTag));
            OnPay.Invoke(buildingTag,itemTag,value);
            
            if(HasUnpaidReceiptItems(buildingTag)) return;
            
            SetUnlock(buildingTag,true);
        }

        public bool HasUnpaidReceiptItems(AKTag buildingTag)
        {
            var receipt = GetReceipt(buildingTag);
            for (int i = 0; i < receipt.Length; i++)
            {
                if(GetPaid(buildingTag,receipt[i].ItemTag)>=GetCost(buildingTag,receipt[i].ItemTag)) continue;

                return true;
            }

            return false;
        }

        public int GetPaid(AKTag buildingTag,AKTag itemTag)
        {
            return GetDataContainer(buildingTag).PaidMapping[itemTag].Value;
        }
        
        public int GetCost(AKTag buildingTag,AKTag itemTag)
        {
            foreach (var receiptItemData in GetData(buildingTag).Receipt)
            {
                if(receiptItemData.ItemTag != itemTag) continue;

                return receiptItemData.Value;
            }
            
            return 0;
        }

        public ItemData[] GetReceipt(AKTag buildingTag)
        {
            return GetData(buildingTag).Receipt;
        }

        public float GetBuildTime(AKTag buildingTag)
        {
            return GetData(buildingTag).TimeToBuild;
        }

        public void SaveStartBuildTime(AKTag buildingTag)
        {
            GetDataContainer(buildingTag).StartBuildTime = DateTime.Now.ToBinary();
            _saveService.Save(buildingTag._Name, GetDataContainer(buildingTag));
        }

        public void IncreaseConstructionProgress(AKTag buildingTag,float value)
        {
            _buildingDataContainerMapping[buildingTag].ConstructionProgress += value;
            _saveService.Save(buildingTag._Name, GetDataContainer(buildingTag));
        }

        public float GetConstructionProgress(AKTag buildingTag)
        {
            return _buildingDataContainerMapping[buildingTag].ConstructionProgress;
        }

        public long GetStartBuildTimeBinary(AKTag buildingTag)
        {
            return GetDataContainer(buildingTag).StartBuildTime;
        }

        private BuildingData GetData(AKTag buildingTag)
        {
            return _buildingDataMapping.ContainsKey(buildingTag) ? _buildingDataMapping[buildingTag] : null;
        }

        private BuildingDataContainer GetDataContainer(AKTag buildingTag)
        {
            return _buildingDataContainerMapping.ContainsKey(buildingTag) ? _buildingDataContainerMapping[buildingTag] : null;
        }
    }
}
