using _Source.Code._AKFramework.AKECS.Runtime;
#if POOL_EXIST
using _Source.Code._AKFramework.AKPools.Runtime;
#endif

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime 
{
    [AKGenerateProvider]
    public struct AKTrigger3D_Active 
#if POOL_EXIST
        : IAKPoolRemove
#endif
    {
        
    }
}