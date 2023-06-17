using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _toolTipAmount;
        [SerializeField] private TextMeshProUGUI _buyButtonStatus;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _capacity;
        [SerializeField] private TextMeshProUGUI _nextLevelCapacity;
        [SerializeField] private TextMeshProUGUI _minMaxValue;

        [SerializeField] private Button _getCurrenciesButton;
        [SerializeField] private GameObject _toolTip;

        public void SetData(bool isBuy, bool isMaxLevel)
        {
            _buyButtonStatus.text = !isBuy ? "Купить" : "Улучшить";
            _getCurrenciesButton.gameObject.SetActive(isBuy);
            _description.gameObject.SetActive(!isBuy);
            _toolTip.SetActive(isBuy);
            _nextLevelCapacity.gameObject.SetActive(isBuy && !isMaxLevel);
        }

        public void SetButtonsStatus(bool isFillMoneyBox, bool isBuy)
        {
            _getCurrenciesButton.interactable = isFillMoneyBox || !isBuy;
        }

        public void SetLevel(int level)
        {
            _levelText.text = $"Ур.{level}";
        }

        public void SetQuantity(int quantity)
        {
            _toolTipAmount.text = $"{quantity}";
        }

        public void SetCapacity(int currentCapacity, int nextLevelCapacity)
        {
            _capacity.text = $"{currentCapacity}";
            _nextLevelCapacity.text = $" +{nextLevelCapacity - currentCapacity}";
        }

        public void SetSlider(int progressDataAmount, int configCapacities)
        {
            _slider.value = (float)progressDataAmount / configCapacities;
            _minMaxValue.text = $"{progressDataAmount}/{configCapacities}";
        }
    }
}