using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public interface IAKEcsRequest
    {
        EcsPackedEntity TargetPackedEntity { get; set; }
    }
}