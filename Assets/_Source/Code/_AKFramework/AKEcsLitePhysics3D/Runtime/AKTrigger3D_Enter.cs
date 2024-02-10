using System.Collections.Generic;
using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public struct AKTrigger3D_Enter : IEcsAutoReset<AKTrigger3D_Enter>
    {
        public HashSet<EcsPackedEntity> Other;

        public void AutoReset(ref AKTrigger3D_Enter c)
        {
            if (c.Other == null)
            {
                c.Other = new HashSet<EcsPackedEntity>();
                return;
            }

            c.Other.Clear();
        }
    }
}