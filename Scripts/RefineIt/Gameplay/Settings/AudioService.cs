using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Infrastructure.StaticData;
using UnityEngine;
using Zenject;

namespace Gameplay.Settings
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        private ISettingModel _settingModel;
        private IStaticDataService _staticDataService;
        private Dictionary<EAudioSources, AudioSource> AudioSources = new();

        public void Initialize(ISettingModel settingModel,IStaticDataService staticDataService)
        {
            _settingModel = settingModel;
            _staticDataService = staticDataService;
            CreateAudioSource();
        }

        private void CreateAudioSource()
        {
            AudioSources.Add(EAudioSources.MusicSource, _musicSource);
            AudioSources.Add(EAudioSources.SFXSource, _sfxSource);
        }
        
        public void PlaySFX(EAudioSources guidSource, ClickClipId idClickClip)
        {
            var clip = _staticDataService.AudioData.AudioClips.Find(x => x.clickClipGuid == idClickClip).Clip;
            AudioSources[guidSource].PlayOneShot(clip);
        }
    }

    public enum EAudioSources
    {
        MusicSource,
        SFXSource
    }
}