using System.Collections.Generic;
using _Client_.Scripts.Interfaces;
using Leopotam.EcsLite;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static bool Has<T>(this EcsWorld world, EcsPackedEntity targetPackedEntity) where T : struct, ISFEcsRequest
        {
            foreach (var entity in world.Filter<T>().End())
            {
                ref var value = ref world.GetPool<T>().Get(entity);

                if (value.TargetPackedEntity.EqualsTo(targetPackedEntity)) return true;
            }

            return false;
        }
        
        public static bool Has<T> (this EcsWorld world, EcsPackedEntity targetPackedEntity, out EcsPackedEntity packedEntity) where T : struct, ISFEcsRequest
        {
            foreach (var entity in world.Filter<T>().End())
            {
                ref var value = ref world.GetPool<T>().Get(entity);

                if (value.TargetPackedEntity.EqualsTo(targetPackedEntity))
                {
                    packedEntity = world.PackEntity(entity);
                    return true;
                }
            }

            packedEntity = default;
            return false;
        }
        
        public static bool Has<T> (this EcsWorld world, EcsPackedEntity targetPackedEntity, out List<EcsPackedEntity> packedEntity) where T : struct, ISFEcsRequest
        {
            packedEntity = new List<EcsPackedEntity>();
            
            foreach (var entity in world.Filter<T>().End())
            {
                ref var value = ref world.GetPool<T>().Get(entity);

                if (value.TargetPackedEntity.EqualsTo(targetPackedEntity))
                {
                    packedEntity.Add(world.PackEntity(entity));
                }
            }
            
            return packedEntity.Count > 0;
        }
    }
}