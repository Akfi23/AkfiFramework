using System;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    [Serializable]
    public struct AKCapsuleTriggerCastEvent
    {
        public Vector3 point1;
        public Vector3 point2;
        public float radius;
    }
}