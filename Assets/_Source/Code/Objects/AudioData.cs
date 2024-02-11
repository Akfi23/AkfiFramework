using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public struct AudioData
    {
        [SerializeField]
        private AKTag tag;
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        private bool ignoreCountLimit;

        [SerializeField]
        private bool changePitch;

        [ShowIf(nameof(changePitch))]
        [SerializeField]
        private float pitchResetTimer;
        [ShowIf(nameof(changePitch))]
        [Range(1, 3)]
        [SerializeField]
        private float maxPitch;
        [ShowIf(nameof(changePitch))]
        [SerializeField]
        private float pitchStep;
        [SerializeField]
        private bool customVolume;
        [ShowIf(nameof(changePitch))]
        [Range(0, 1)]
        [SerializeField]
        private float volume;

        public AKTag Tag => tag;
        public AudioClip AudioClip => audioClip;
        public bool IgnoreCountLimit => ignoreCountLimit;
        public bool ChangePitch => changePitch;
        public float PitchResetTimer => pitchResetTimer;
        public float MaxPitch => maxPitch;
        public float PitchStep => pitchStep;
        public bool CustomVolume => customVolume;
        public float Volume => volume;
    }

    public struct BusyAudioSourceData
    {
        public AudioSource AudioSource;
        public AKTag SfxTag;
    }

    [Serializable]
    public class PitchData
    {
        public float Timer { get; set; }
        public float MaxPitch { get; }
        public float PitchStep { get; }

        public float CurrentPitch { get; set; } = 1f;

        public PitchData(float timer, float maxPitch, float pitchStep)
        {
            Timer = timer;
            MaxPitch = maxPitch;
            PitchStep = pitchStep;
        }
    }
}