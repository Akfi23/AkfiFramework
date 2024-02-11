using System;

namespace _Client_.Scripts.Objects
{
    [Serializable]
    public class SettingsDataContainer
    {
        public bool isVibrations;
        public bool isMusic;
        public bool isSfx;

        public SettingsDataContainer(bool vibro = true, bool music = true, bool sfx = true)
        {
            isVibrations = vibro;
            isMusic = music;
            isSfx = sfx;
        }
    }
}