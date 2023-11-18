#if ECS_EXIST

using System;
using _Source.Code._AKFramework.AKECS.Runtime;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    [AKGenerateProvider]
    [Serializable]
    public struct AKTagRef
    {
        public AKTag[] tags;
    }
}
#endif