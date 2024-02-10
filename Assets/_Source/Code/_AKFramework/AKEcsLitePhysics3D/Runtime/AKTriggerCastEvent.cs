using System;
using System.Collections.Generic;
using _Source.Code._AKFramework.AKECS.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [Serializable]
    public struct AKTriggerCastEvent : IEcsInit<AKTriggerCastEvent>
    {
        public int hitCount;
        public LayerMask layerMask;
        
        [HideInInspector]
        public Collider[] colliders;
        [HideInInspector]
        public int count;
        public Dictionary<EcsPackedEntity, bool> entitiesDictionary;
        public HashSet<EcsPackedEntity> enterHashSet;
        public HashSet<EcsPackedEntity> stayHashSet;
        public HashSet<EcsPackedEntity> exitHashSet;
        
        public void Init(ref AKTriggerCastEvent c)
        {
            c.colliders = new Collider[c.hitCount];
            c.entitiesDictionary = new Dictionary<EcsPackedEntity, bool>();
            c.entitiesDictionary.EnsureCapacity(c.hitCount);
            c.enterHashSet = new HashSet<EcsPackedEntity>();
            c.stayHashSet = new HashSet<EcsPackedEntity>();
            c.exitHashSet = new HashSet<EcsPackedEntity>();
        }
    }
}