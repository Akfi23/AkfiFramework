using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public struct VibrationData
    {
        [SerializeField]
        private AKTag tag;
        // [SerializeField]
        // private HapticPatterns.PresetType presetType;

        public AKTag Tag => tag;
        // public HapticPatterns.PresetType PresetType => presetType;
    }
}