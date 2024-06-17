using System;
using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using _Source.Code.Objects;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Services
{
    public class ItemService : IItemService<int>
    {
        [AKInject] 
        private ItemsDatabase _itemsDatabase;
        [AKInject] 
        private ISaveService _saveService;
        [AKInject]
        private UpgradesService _upgradesService;

        public Action<AKTag> OnItemChange { get; set; } = delegate {  };
        public Action<AKTag, int> OnItemAdd { get; set; } = delegate {  };
        public Action<AKTag, int> OnItemRemove { get; set; } = delegate {  };

        private readonly Dictionary<AKTag, IItem> _itemsMapping = new();

        private IItem _item;

        [AKInject]
        private void Init()
        {
            foreach (var item in _itemsDatabase.Items)
            {
                var load = _saveService.Load($"{item.ItemTag}", $"0_{item.IsUnlock}").Split("_");
                var loadTotalCollectedCount = _saveService.Load($"TotalCollected_{item.ItemTag}", 0);
                item.Init(int.Parse(load[0]), loadTotalCollectedCount, bool.Parse(load[1]));
                _itemsMapping.Add(item.ItemTag, item);
            }
            
            // AKDebug.Log(Get(AKTags.Items__Money));
            // Add(AKTags.Items__Money,100);
        }
        
        public void Add(AKTag tag, int value)
        {
            _item = GetItem(tag);
            _item.Add(value);
            _saveService.Save($"{_item.ItemTag}", $"{_item.Get()}_{_item.IsUnlock}");
            _saveService.Save($"TotalCollected_{_item.ItemTag}", _item.GetTotalCollected());
            OnItemChange.Invoke(tag);
            OnItemAdd.Invoke(tag, value);
        }

        public void Remove(AKTag tag, int value)
        {
            _item = GetItem(tag);
            _item.Add(-value);
            _saveService.Save($"{_item.ItemTag}", $"{_item.Get()}_{_item.IsUnlock}");
            OnItemChange.Invoke(tag);
            OnItemRemove.Invoke(tag, value);
        }

        public int Get(AKTag tag)
        {
            _item = GetItem(tag);
            return _item.Get();
        }

        public int GetTotalCollected(AKTag tag)
        {
            _item = GetItem(tag);
            return _item.GetTotalCollected();
        }

        public bool IsUnlock(AKTag tag)
        {
            return GetItem(tag).IsUnlock;
        }

        public void SetUnlock(AKTag tag, bool state)
        {
            _item = GetItem(tag);
            _item.SetUnlock(state);
            _saveService.Save($"{_item.ItemTag}", $"{_item.Get()}_{_item.IsUnlock}");
            OnItemChange.Invoke(tag);
        }

        public AKPrefab GetPrefab(AKTag tag)
        {
            _item = GetItem(tag);
            return _item.GetPrefab();
        }

        private IItem GetItem(AKTag tag)
        {
            return _itemsMapping.ContainsKey(tag) ? _itemsMapping[tag] : null;
        }

        public Sprite GetIcon(AKTag tag)
        {
            _item = GetItem(tag);
            return _item.Icon;
        }

        public bool HasItem(AKTag tag)
        {
            return _itemsMapping.ContainsKey(tag);
        }

        public bool HasCapacity(AKTag tag)
        {
            return GetItem(tag).HasCapacity;
        }
        
    }
}