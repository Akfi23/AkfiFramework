using System.Collections.Generic;
using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public static class AKEcsLiteExtensions
    {
        public static ref T Replace<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
            {
                return ref pool.Get(entity);
            }

            return ref pool.Add(entity);
        }

        public static IEcsSystems DeleteHereAll<T>(this IEcsSystems systems) where T : struct
        {
            return systems.Add(new DeleteHereAllSystem<T>());
        }

        public static IEcsSystems TimerHere<T>(this IEcsSystems systems) where T : struct, IAKEcsTimer
        {
            return systems.Add(new TimerHereSystem<T>());
        }

        public static IEcsSystems TimerHere<T, TK>(this IEcsSystems systems)
            where T : struct, IAKEcsTimer where TK : struct
        {
            return systems.Add(new TimerHereCallbackSystem<T, TK>());
        }

        public static bool Has<T>(this EcsWorld world, EcsPackedEntity targetPackedEntity)
            where T : struct, IAKEcsRequest
        {
            foreach (var entity in world.Filter<T>().End())
            {
                ref var value = ref world.GetPool<T>().Get(entity);

                if (value.TargetPackedEntity.EqualsTo(targetPackedEntity)) return true;
            }

            return false;
        }

        public static bool Has<T>(this EcsWorld world, EcsPackedEntity targetPackedEntity,
            out EcsPackedEntity packedEntity) where T : struct, IAKEcsRequest
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

        public static bool Has<T>(this EcsWorld world, EcsPackedEntity targetPackedEntity,
            out List<EcsPackedEntity> packedEntity) where T : struct, IAKEcsRequest
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