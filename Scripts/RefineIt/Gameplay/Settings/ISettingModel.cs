using Gameplay.Settings;
using UnityEngine;

namespace Gameplay.Settings
{
    public interface ISettingModel
    {
        void Initialize();
        void SetSFXVolume(float value);
        void SetMusicVolume(float value);
        bool SFXStatus();
        bool MusicStatus();
    }
}