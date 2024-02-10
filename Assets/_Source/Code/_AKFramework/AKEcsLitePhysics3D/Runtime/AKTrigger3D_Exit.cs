using System.Collections.Generic;
using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public struct AKTrigger3D_Exit : IEcsAutoReset<AKTrigger3D_Exit>
    {
        public HashSet<EcsPackedEntity> Other;

        public void AutoReset(ref AKTrigger3D_Exit c)
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