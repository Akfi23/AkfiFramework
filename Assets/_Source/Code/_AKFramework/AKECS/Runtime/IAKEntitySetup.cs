using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public interface IAKEntitySetup 
    {
        void Setup(ref EcsWorld world, ref EcsPackedEntity packedEntity);

    }
}
