using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [Serializable]
    public sealed class AKScreen : AKType
    {
        public AKScreen(int id, string name) : base(id, name)
        {
        }
        
        public AKScreen()
        {
        }
    }
}