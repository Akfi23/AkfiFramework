using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [Serializable]
    public class AKScenesGroup : AKType
    {
        public AKScenesGroup(int id, string _name) : base(id, _name)
        {
            
        }
        
        public AKScenesGroup()
        {
        }
    }
}