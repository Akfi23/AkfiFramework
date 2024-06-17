using System;
using _Source.Code._AKFramework.AKCore.Runtime;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [Serializable]
    public class AKScene : AKType
    {
        public AKScene(int id, string name) : base(id, name)
        {
        }
        
        public AKScene()
        {
        }
    }
}