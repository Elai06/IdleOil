using System.Globalization;
using Gameplay.MVVM.Views;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Workspaces.MiningWorkspace.View
{
    public class MiningView : MonoBehaviour
    {
        private const string SEC = "сек";
        
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private IconDescriptionView _priceView;
        [SerializeField] private RectTransform _notBuildedView;
        [SerializeField] private Image _resourceImage;
        [SerializeField] private TextMeshProUGUI _resourceValue;
        [SerializeField] private TextMeshProUGUI _durationValue;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _sliderValue;

        public void ViewResource(Sprite image, int value, float duration)
        {
            _resourceImage.sprite = image;
            _resourceValue.text = value.ToString();
            _durationValue.text = duration.ToString(CultureInfo.InvariantCulture) + SEC;
        }
        
        public void ViewBuilded(bool isBuilded)
        {
            _notBuildedView.gameObject.SetActive(!isBuilded);
        }

        public void ViewMiningCost(Sprite sprite, string price)
        {
            _priceView.ViewSprite(sprite);
            _priceView.ViewValue(price);
        }

        public void ViewData(Sprite sprite, string description)
        {
            UpdateSprite(sprite);
            ViewDescription(description);
        }

        private void ViewDescription(string description)
        {
            _description.text = description;
        }

        public void UpdateSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SliderView(int capacity, int value)
        {
            _slider.maxValue = capacity;
            _slider.value = value;
            _sliderValue.text = $"{value}/{capacity}";
        }

        public void UpdateSliderValue(int value)
        {
            _slider.value = value;
            _sliderValue.text = $"{value}/{_slider.maxValue}";

        }
    }
}