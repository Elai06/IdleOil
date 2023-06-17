using Gameplay.PromoCode;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.Settings.UI
{
    public class SettingWindow : Window
    {
        [SerializeField] private SettingViewInitializer _settingViewInitializer;
        [SerializeField] private PromoCodeInitializer _promoCodeInitializer;
        private ISettingModel _settingModel;
        private PromoCodeModel _promoCodeModel;

        [Inject]
        private void Construct(ISettingModel settingModel, PromoCodeModel promoCodeModel)
        {
            _settingModel = settingModel;
            _promoCodeModel = promoCodeModel;
        }

        private void OnEnable()
        {
            _promoCodeInitializer.Initialize(_promoCodeModel);
            _settingViewInitializer.Initialize(_settingModel);
        }
    }
}