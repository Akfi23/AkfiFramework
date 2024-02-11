using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEngine;

namespace _Source.Code.Interfaces
{
    public interface IAudioService
    {
        void PlaySound(AKTag sfxTag,
            bool loop,
            Transform transformTarget,
            float spatialBlend,
            AudioRolloffMode rolloffMode,
            float minDistance,
            float maxDistance);

        void PlayMusic();
        void StopAllLoopSounds();
        void StopMusic();
        void Update();
    }
}