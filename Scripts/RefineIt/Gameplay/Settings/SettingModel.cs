using System.Collections.Generic;
using Gameplay.Settings;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using UnityEngine;

namespace Gameplay.Settings
{
    public class SettingModel : ISettingModel
    {
        private IProgressService _progressService;
        private IStaticDataService _staticDataService;
        private const string MIXER_MUSIC = "Music";
        private const string MIXER_SFX = "SFX";

        SettingModel(IProgressService progressService, IStaticDataService staticDataService)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            SetMusicVolume(_progressService.PlayerProgress.MusicVolume);
            SetSFXVolume(_progressService.PlayerProgress.SFXVolume);
        }

        public void SetMusicVolume(float value)
        {
            _staticDataService.AudioData.Mixer.SetFloat(MIXER_MUSIC, value );
            _progressService.PlayerProgress.MusicVolume = value;
        }

        public void SetSFXVolume(float value)
        {
            _staticDataService.AudioData.Mixer.SetFloat(MIXER_SFX, value);
            _progressService.PlayerProgress.SFXVolume = value;
        }
        
        public bool MusicStatus()
        {
            if (_progressService.PlayerProgress.MusicVolume >= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool SFXStatus()
        {
            if (_progressService.PlayerProgress.SFXVolume >= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}