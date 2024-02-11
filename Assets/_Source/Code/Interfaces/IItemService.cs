using System;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEngine;

namespace _Source.Code.Interfaces
{
    public interface IItemService<T>
    {
        Action<AKTag> OnItemChange { get; set; }
        Action<AKTag, T> OnItemAdd { get; set; }
        Action<AKTag, T> OnItemRemove { get; set; }
        void Add(AKTag tag, T value);
        void Remove(AKTag tag, T value);
        T Get(AKTag tag);
        T GetTotalCollected(AKTag tag);
        bool IsUnlock(AKTag tag);
        void SetUnlock(AKTag tag, bool state);
        AKPrefab GetPrefab(AKTag tag);
        Sprite GetIcon(AKTag tag);
        bool HasItem(AKTag tag);
        bool HasCapacity(AKTag tag);
    }
}
