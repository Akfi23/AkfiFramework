using System;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public class ValueItem : IItem
    {
        [SerializeField]
        [AKTagsGroup("Items")]
        private AKTag itemTag;
        public AKTag ItemTag => itemTag;
        [SerializeField, ReadOnly]
        private int count = 0;
        [SerializeField, ReadOnly]
        private int totalCollectedCount = 0;
        [SerializeField]
        private AKPrefab prefab;
        [SerializeField][PreviewField(50, ObjectFieldAlignment.Left)]
        private Sprite icon;
        public Sprite Icon => icon;
        [SerializeField] 
        private bool isUnlock;
        public bool IsUnlock => isUnlock;
        [SerializeField] 
        private bool hasCapacity;
        
        public bool HasCapacity => hasCapacity;
        
        public void Init(int value, int totalCollectedValue, bool state)
        {
            count = value;
            totalCollectedCount = totalCollectedValue;
            isUnlock = state;
        }

        public void Add(int value)
        {
            count += value;
            if (value >=0)
                totalCollectedCount += value;
        }

        public int Get()
        {
            return count;
        }

        public int GetTotalCollected()
        {
            return totalCollectedCount;
        }
        
        public AKPrefab GetPrefab()
        {
            return prefab;
        }

        public void SetUnlock(bool state)
        {
            isUnlock = state;
        }
    }
}