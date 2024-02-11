using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public struct VFXSpawnData
    {
        //[SFTagsGroup("VFX")]
        public AKTag vfxTag;
        public bool isCountRange;
        public int count;
        [ShowIf("isCountRange")]
        public int maxCount;
        public float spawnRadius;
        public float offsetY;
        public bool isSpecialPosition;
        [ShowIf("isSpecialPosition")]
        public Vector3 specialPosition;
        public bool isSpecialRotation;
        [ShowIf("isSpecialRotation")]
        public Vector3 rotation;
        // public float delayBetweenSpawns;
        // public bool withAnim;
        // [ShowIf("withAnim")]
        // public SpawnAnimData animData;
    }
}