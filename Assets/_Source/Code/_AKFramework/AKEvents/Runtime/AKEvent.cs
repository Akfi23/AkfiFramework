using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKEvents.Runtime
{
    [Serializable]
    public class AKEvent : AKType
    {
        public AKEvent(int id, string name) : base(id, name)
        {
        }
        
        public AKEvent()
        {
        }
    }
}