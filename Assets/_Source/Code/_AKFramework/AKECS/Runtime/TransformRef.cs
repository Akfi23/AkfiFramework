using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public struct TransformRef 
    {
        public Transform instance;
        public Vector3 InitialPosition;
        public Quaternion InitialRotation;
        public Vector3 InitialScale;
    }
}