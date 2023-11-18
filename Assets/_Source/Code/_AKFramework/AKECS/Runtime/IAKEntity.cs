using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public interface IAKEntity 
    {
        EcsWorld World { get; }
        EcsPackedEntity EcsPackedEntity { get; }
    }
}
