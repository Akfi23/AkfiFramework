using System.Collections.Generic;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using _Source.Code.Objects;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Source.Code.Services
{
    public class AudioService : IAudioService
    {
        [Inject]
        private SettingsService _settingsService;
        [Inject]
        private AudioDatabase _audioDatabase;

        private readonly List<AudioSource> _freeAudioSources = new();
        private readonly List<BusyAudioSourceData> _busyAudioSources = new();
        private readonly Dictionary<AKTag, PitchData> _pitchData = new();
        private readonly HashSet<AKTag> _currentlyPlayedAudioSources = new();

        private Transform _audioSourcesRoot;
        private AudioSource _musicAudioSource;
        private AKTag[] _musicTags;

        private int _audioSourceNum;
        private int _currentMusicIndex;
        private Tween _playNextMusicTween;

        [Inject]
        private void Init()
        {
            _audioSourcesRoot = new GameObject("[AUDIO_SOURCES_ROOT]").transform;

            _musicAudioSource = new GameObject("[MUSIC_AUDIO_SOURCE]").AddComponent<AudioSource>();
            _musicAudioSource.transform.parent = _audioSourcesRoot;
            _musicAudioSource.loop = true;
            _musicAudioSource.playOnAwake = false;

            _settingsService.OnSFXSettingChanged += OnSFXSettingsChanged;
            _settingsService.OnMusicSettingChanged += OnMusicSettingsChanged;
        }

        public void PlaySound(AKTag sfxTag,
            bool loop = false,
            Transform transformTarget = null,
            float spatialBlend = 0f,
            AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic,
            float minDistance = 1f,
            float maxDistance = 25f)
        {
            if (!_settingsService.IsSFX) return;
            var audioData = _audioDatabase.GetAudioData(sfxTag);
            if (!audioData.IgnoreCountLimit && _currentlyPlayedAudioSources.Contains(sfxTag)) return;

            var audioSource = GetFreeAudioSource();
            SetupAudioSource(audioSource, loop, spatialBlend, rolloffMode, minDistance, maxDistance);

            var audioSourceTransform = audioSource.transform;
            audioSourceTransform.parent = transformTarget ? transformTarget : _audioSourcesRoot;
            audioSourceTransform.localPosition = Vector3.zero;

            _busyAudioSources.Add(new BusyAudioSourceData()
            {
                AudioSource = audioSource,
                SfxTag = sfxTag
            });

            PlayAudioSource(audioSource, audioData);
        }

        private AudioSource GetFreeAudioSource()
        {
            if (_freeAudioSources.Count <= 0)
            {
                var newAudioSource = CreateAudioSource();
                SetupAudioSource(newAudioSource);
                _freeAudioSources.Add(newAudioSource);
            }

            var freeAudioSource = _freeAudioSources[^1];
            _freeAudioSources.RemoveAt(_freeAudioSources.Count - 1);

            return freeAudioSource;
        }

        private AudioSource CreateAudioSource()
        {
            var newAudioSource = new GameObject($"[SFX_AUDIO_SOURCE_{_audioSourceNum}]").AddComponent<AudioSource>();
            _audioSourceNum++;
            return newAudioSource;
        }

        private void SetupAudioSource(AudioSource audioSource,
            bool loop = false,
            float spatialBlend = 0f,
            AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic,
            float minDistance = 1f,
            float maxDistance = 25f)
        {
            audioSource.transform.parent = _audioSourcesRoot;
            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            audioSource.spatialBlend = spatialBlend;
            audioSource.rolloffMode = rolloffMode;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }

        public void StopAllLoopSounds()
        {
            foreach (var busyAudioSource in _busyAudioSources)
            {
                if (busyAudioSource.AudioSource.loop)
                {
                    busyAudioSource.AudioSource.Stop();
                }
            }
        }

        public void StopMusic()
        {
            _musicAudioSource.Stop();
            _playNextMusicTween?.Kill();
        }

        public void PlayMusic()
        {
            if (!_settingsService.IsMusic) return;

            var audioData = _audioDatabase.GetAudioData(_musicTags[_currentMusicIndex]);

            _musicAudioSource.pitch = 1f;
            if (_musicAudioSource.isPlaying) _musicAudioSource.Stop();
            _musicAudioSource.clip = audioData.AudioClip;
            if (audioData.CustomVolume) _musicAudioSource.volume = audioData.CustomVolume ? audioData.Volume : 1f;

            _musicAudioSource.Play();

            _playNextMusicTween?.Kill();
            _playNextMusicTween = DOTween.Sequence()
                .AppendInterval(audioData.AudioClip.length)
                .AppendCallback(() =>
                {
                    _currentMusicIndex = (_currentMusicIndex + 1) % _musicTags.Length;
                    PlayMusic();
                });
        }

        private void PlayAudioSource(AudioSource source, AudioData data)
        {
            if (data.ChangePitch)
            {
                if (!_pitchData.ContainsKey(data.Tag))
                {
                    _pitchData.Add(data.Tag, new PitchData(data.PitchResetTimer, data.MaxPitch, data.PitchStep));
                }

                source.pitch = _pitchData[data.Tag].CurrentPitch;

                var pitchData = _pitchData[data.Tag];
                _pitchData[data.Tag].CurrentPitch = Mathf.Clamp(pitchData.CurrentPitch + pitchData.PitchStep, 1f, pitchData.MaxPitch);
                _pitchData[data.Tag].Timer = data.PitchResetTimer;
            }
            else
            {
                source.pitch = 1f;
            }

            if (source.isPlaying) source.Stop();
            source.clip = data.AudioClip;
            source.volume = data.CustomVolume ? data.Volume : 1f;

            _currentlyPlayedAudioSources.Add(data.Tag);

            source.Play();
        }

        public void Update()
        {
            UpdatePitch();
            CheckBusyAudioSources();
        }

        private void UpdatePitch()
        {
            AKTag removePitchData = null;

            foreach (var pitchData in _pitchData)
            {
                if (pitchData.Value.Timer > 0) pitchData.Value.Timer -= Time.deltaTime;
                else removePitchData = pitchData.Key;
            }

            if (removePitchData != null)
            {
                _pitchData.Remove(removePitchData);
            }
        }

        private void CheckBusyAudioSources()
        {
            var removeIndex = -1;
            for (var i = 0; i < _busyAudioSources.Count; i++)
            {
                if (!_busyAudioSources[i].AudioSource.isPlaying)
                {
                    removeIndex = i;
                    break;
                }
            }

            if (removeIndex >= 0)
            {
                var removedAudioSource = _busyAudioSources[removeIndex];
                _busyAudioSources.RemoveAt(removeIndex);
                SetupAudioSource(removedAudioSource.AudioSource);
                _freeAudioSources.Add(removedAudioSource.AudioSource);
                removedAudioSource.AudioSource.transform.parent = _audioSourcesRoot;
                _currentlyPlayedAudioSources.Remove(removedAudioSource.SfxTag);
            }
        }

        private void OnSFXSettingsChanged(bool state)
        {
            if (!state) StopAllLoopSounds();
        }

        private void OnMusicSettingsChanged(bool state)
        {
            if (!state) StopMusic();
            else PlayMusic();
        }
    }
}