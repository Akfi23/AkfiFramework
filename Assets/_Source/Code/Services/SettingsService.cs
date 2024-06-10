using System;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code.Interfaces;

namespace _Source.Code.Services
{
    public class SettingsService : IAKService
    {
        [AKInject]
        private ISaveService _saveService;
        // [Inject]
        // private ISFAnalyticsService _analyticsService;

        public bool IsVibrations => _container.isVibrations;
        public bool IsMusic => _container.isMusic;
        public bool IsSFX => _container.isSfx;

        public Action<bool> OnVibrationsSettingChanged = delegate { };
        public Action<bool> OnSFXSettingChanged = delegate { };
        public Action<bool> OnMusicSettingChanged = delegate { };

        private SettingsDataContainer _container;

        [AKInject]
        private void Init()
        {
            _container = _saveService.Load("Settings", new SettingsDataContainer());
        }

        public void SetMusicState(bool state)
        {
            if (_container.isMusic == state) return;
            _container.isMusic = state;
            OnMusicSettingChanged(state);
            SaveSettings();
        }

        public void SetSFXState(bool state)
        {
            if (_container.isSfx == state) return;
            _container.isSfx = state;
            OnSFXSettingChanged(state);
            SaveSettings();
        }

        public void SetVibrationState(bool state)
        {
            if (_container.isVibrations == state) return;
            _container.isVibrations = state;
            OnVibrationsSettingChanged(state);
            SaveSettings();
        }

        private void SaveSettings() => _saveService.Save("Settings", _container);
    }
}