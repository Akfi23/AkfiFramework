using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [Serializable]
    public sealed class AKToggle : AKType
    {
        public AKToggle(int id, string name) : base(id, name)
        {
        }
        
        public AKToggle()
        {
        }
    }
}