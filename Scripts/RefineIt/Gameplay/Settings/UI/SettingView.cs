using System;
using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Gameplay.Settings.UI
{
    public class SettingView : MonoBehaviour
    {
        [SerializeField] private Button _buttonLanguage;
        [SerializeField] private Button _buttonRate;
        [SerializeField] private Button _textLicence;

        [SerializeField] public Toggle ToggleSFX;
        [SerializeField] public Toggle ToggleMusic;

        private void Start()
        {
            _buttonLanguage.onClick.AddListener(ButtonLanguage);
            _buttonRate.onClick.AddListener(ButtonRate);
            _textLicence.onClick.AddListener(ButtonLicence);
        }

        private void ButtonLanguage()
        {
            Debug.Log("Btn Language");
        }

        private void ButtonRate()
        {
            Debug.Log("Btn Rate");
        }

        private void ButtonLicence()
        {
            Debug.Log("Btn Licence");
        }
    }

    public static class GamesResourcesName
    {
        public static string GetResourceName(string resourceName)
        {
            if (resourceName == ResourceType.Oil.ToString())
            {
                return "Нефть";
            }

            if (resourceName == ResourceType.Bitumen.ToString())
            {
                return "Битум";
            }

            if (resourceName == ResourceType.Coke.ToString())
            {
                return "Кокс";
            }

            if (resourceName == ResourceType.Nafta.ToString())
            {
                return "Нафта";
            }

            if (resourceName == ResourceType.Goodron.ToString())
            {
                return "Гудрон";
            }

            if (resourceName == ResourceType.Dtvs.ToString())
            {
                return "ДТВС";
            }

            if (resourceName == ResourceType.Ethanol.ToString())
            {
                return "Этанол";
            }

            if (resourceName == ResourceType.Diesel.ToString())
            {
                return "Дизель";
            }

            if (resourceName == ResourceType.Olefin.ToString())
            {
                return "Олефины";
            }

            if (resourceName == ResourceType.Plastic.ToString())
            {
                return "Пластик";
            }

            if (resourceName == ResourceType.Vg.ToString())
            {
                return "Вг";
            }

            if (resourceName == ResourceType.EngineOil.ToString())
            {
                return "Моторное масло";
            }

            if (resourceName == ResourceType.SportsFuel.ToString())
            {
                return "Спортивное масло";
            }

            return "";
        }
    }
}