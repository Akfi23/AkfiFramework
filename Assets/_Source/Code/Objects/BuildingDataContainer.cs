using System;
using System.Collections.Generic;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public class BuildingDataContainer
    {
        [SerializeField]
        public bool IsUnlock;
        [SerializeField]
        public ItemData[] Paid;
        [SerializeField]
        public bool IsBuilt;
        [SerializeField]
        public long StartBuildTime;
        [SerializeField] 
        public float ConstructionProgress;
        [NonSerialized]
        public Dictionary<AKTag, ItemData> PaidMapping = new();

        public BuildingDataContainer(bool isUnlock, bool isBuilt)
        {
            IsUnlock = isUnlock;
            IsBuilt = isBuilt;
        }

        public override string ToString()
        {
            var tmp = "";
            foreach (var p in Paid)
            {
                tmp += p + "\n";
            }

            return tmp;
        }
    }
}