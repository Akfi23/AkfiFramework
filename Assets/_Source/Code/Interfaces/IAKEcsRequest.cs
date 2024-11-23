using Leopotam.EcsLite;

namespace _Client_.Scripts.Interfaces
{
    public interface IAKEcsRequest
    {
        EcsPackedEntity TargetPackedEntity { get; set; }
    }
}