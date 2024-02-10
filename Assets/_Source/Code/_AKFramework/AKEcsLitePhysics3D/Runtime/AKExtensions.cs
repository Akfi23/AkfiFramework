using System.Collections.Generic;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime
{
    public static partial class AKExtensions
    {
        public static HashSet<T> AddRange<T>(this HashSet<T> hashSet, HashSet<T> range)
        {
            foreach (var r in range)
            {
                if(!hashSet.Contains(r))
                    hashSet.Add(r);
            }

            return hashSet;
        }
    }
}