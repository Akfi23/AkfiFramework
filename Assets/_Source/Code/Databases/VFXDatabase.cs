using System;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_vfx", menuName = "Game/Databases/VFX")]
    public class VFXDatabase : AKDatabase
    {
        public override string Title => "VFX";

        [SerializeField] 
        private VFXData[] vfxData;
        public VFXData[] VFXData => vfxData;
    }

    [Serializable]
    public class VFXData
    {
        [SerializeField] 
        // [SFTagsGroup("VFX")]
        private AKTag vfxTag;
        [SerializeField]
        private AKPrefab vfxPrefab;

        public AKTag VFXTag => vfxTag;
        public AKPrefab VFXPrefab => vfxPrefab;
    }
}
