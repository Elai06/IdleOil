using System.Threading.Tasks;
using Infrastructure.Windows.MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Settings.UI
{
    public class SettingViewModel : ViewModelBase<ISettingModel, SettingView>
    {
        private const float OFF_MUSIC = -80f;
        private const float OFF_SFX = -80f;
        private const float ON_MUSIC = 0f;
        private const float ON_SFX = 0f;

        private Toggle _toggleSFX;
        private Toggle _toggleMusic;
        public SettingViewModel(ISettingModel model, SettingView view) : base(model, view)
        {
            
        }

        public override async Task Show()
        {
            LoadToggles();
        }

        private void LoadToggles()
        {
            _toggleSFX = View.ToggleSFX;
            _toggleMusic = View.ToggleMusic;
            _toggleSFX.isOn = Model.SFXStatus();
            _toggleMusic.isOn = Model.MusicStatus();
            _toggleSFX.onValueChanged.AddListener(SFXChange);
            _toggleMusic.onValueChanged.AddListener(MusicChange);
        }

        private void MusicChange(bool isActive)
        {
            if (isActive)
            {
                Model.SetMusicVolume(ON_MUSIC);
            }
            else
            {
                Model.SetMusicVolume(OFF_MUSIC);
            }
        }
        
        private void SFXChange(bool isActive)
        {
            if (isActive)
            {
                Model.SetSFXVolume(ON_SFX);
            }
            else
            {
                Model.SetSFXVolume(OFF_SFX);
            }
        }
    }
}