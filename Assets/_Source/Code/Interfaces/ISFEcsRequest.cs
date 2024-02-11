using Leopotam.EcsLite;

namespace _Client_.Scripts.Interfaces
{
    public interface ISFEcsRequest
    {
        EcsPackedEntity TargetPackedEntity { get; set; }
    }
}