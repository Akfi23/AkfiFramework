using Leopotam.EcsLite;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static ref T SafeAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity)) return ref pool.Add(entity);
            return ref pool.Get(entity);
        }
        
        public static void SafeDel<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity)) return;
            
            pool.Del(entity);
        }
    }
}