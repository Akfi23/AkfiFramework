using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEngine;

namespace _Source.Code.Objects
{
    public interface IItem
    {
        AKTag ItemTag { get; }
        Sprite Icon { get; }
        bool IsUnlock { get; }
        void Init(int value, int totalCollectedValue, bool state);
        void Add(int value);
        int Get();
        int GetTotalCollected();
        bool HasCapacity { get; }
        AKPrefab GetPrefab();
        void SetUnlock(bool state);
    }
}