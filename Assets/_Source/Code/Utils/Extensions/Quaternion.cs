using UnityEngine;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static Quaternion GetLookAtYRotation(this Transform transform, Vector3 target)
        {
            var lookPos = target - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            return rotation;
        }
    }
}