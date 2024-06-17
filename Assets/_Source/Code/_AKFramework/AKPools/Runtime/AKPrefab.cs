using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    [Serializable]
    public class AKPrefab : AKType
    {
        public AKPrefab(int id, string name) : base(id, name)
        {
        }
        
        public AKPrefab()
        {
        }
    }
}