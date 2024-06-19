using System;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._AKFramework.AKEvents.Runtime;
using _Source.Code._AKFramework.AKPools.Runtime;
using _Source.Code._AKFramework.AKScenes.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime;

namespace _Source.Code.ECS.Components
{
    [AKGenerateProvider]
    [Serializable]
    public struct Player
    {
        public AKTag tag;
        public AKScene scene;
        public AKScreen screen;
        public AKEvent AkEvent;
        public AKPrefab prefab;
    }
}