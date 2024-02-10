using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public struct AKCollision3D_Enter : IEcsAutoReset<AKCollision3D_Enter>
    {
        public HashSet<EcsPackedEntity> Other;
        public List<Collision> Collisions;
        
        public void AutoReset(ref AKCollision3D_Enter c)
        {
            if (c.Other == null)
            {
                c.Other = new HashSet<EcsPackedEntity>();
                c.Collisions = new List<Collision>();
                return;
            }

            c.Collisions.Clear();
            c.Other.Clear();
        }
    }
}
