using System;
using _Source.Code._AKFramework.AKECS.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [AKGenerateProvider]
    [Serializable]
    public struct RigidbodyRef
    {
        public Rigidbody instance;
    }
}
