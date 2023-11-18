using System;
using Leopotam.EcsLite;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public interface IAKWorldService : IAKService
    {
        EcsWorld Default { get; }
        public EcsWorld GetWorld(Guid guid);
        public EcsWorld CreateWorld(out Guid guid, bool started = true);
        public void DestroyWorld(Guid guid);

        public void ResetDefaultWorld();
    }
}