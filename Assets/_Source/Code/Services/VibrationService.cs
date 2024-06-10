using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using UnityEngine;

namespace _Source.Code.Services
{
    public class VibrationService : IVibrationService
    {
        [AKInject]
        private VibrationsDatabase _vibrationsDatabase;
        [AKInject]
        private SettingsService _settingsService;

        private float PrevVibroTime { get; set; }

        public void PlayVibro(AKTag vibroTag, float delay = 0f)
        {
            if (!_settingsService.IsVibrations) return;
            if (Time.realtimeSinceStartup - PrevVibroTime <= delay) return;

            var vibrationData = _vibrationsDatabase.GetVibrationData(vibroTag);

            // HapticPatterns.PlayPreset(vibrationData.PresetType);
            PrevVibroTime = Time.realtimeSinceStartup;
        }
    }
}