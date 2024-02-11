using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [Serializable]
    public class AKButton : AKType
    {
        public AKButton(string id, string name) : base(id, name)
        {
        }
        
        public AKButton()
        {
        }
    }
}