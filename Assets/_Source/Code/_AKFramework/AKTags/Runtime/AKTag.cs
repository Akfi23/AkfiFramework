using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    [Serializable]
    public class AKTag : AKType
    {
        public AKTag(string id, string name) : base(id, name)
        {
        }
        
        public AKTag()
        {
        }
    }
}