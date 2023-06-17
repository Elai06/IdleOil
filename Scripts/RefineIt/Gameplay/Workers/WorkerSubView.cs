using System;
using Gameplay.Region;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Workers
{
    public class WorkerSubView : SubView<WorkersViewData>
    {
        public event Action<string> Click;

        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _bonusValue;
        [SerializeField] private TextMeshProUGUI _nextBonusValue;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _status;

        [SerializeField] private Image _workerImage;
        [SerializeField] private Image _resourceImage;
        [SerializeField] private Image _priceImage;

        [SerializeField] private Button _buyButton;

        [SerializeField] private Button _notAvailableButton;

        public override void Initialize(WorkersViewData data)
        {
            _name.text = data.Name;
            _nextBonusValue.text = data.NextBonusValue;
            _bonusValue.text = data.BonusValue;
            _level.text = data.Level;
            _price.text = data.Price;

            _status.text = data.IsBuy && !data.IsMaxLevel ? "Улучшить" : "Нанять";
            
            if (data.IsMaxLevel)
            {
                _status.text = "Макс.Уровень";
            }

            _workerImage.sprite = data.WorkerImage;
            _resourceImage.sprite = data.ResourceImage;
            _priceImage.sprite = data.PriceImage;

            _notAvailableButton.gameObject.SetActive(!data.IsAvailable);
            _buyButton.gameObject.SetActive(data.IsAvailable);

            _level.gameObject.transform.parent.gameObject.SetActive(data.IsBuy);
            _nextBonusValue.gameObject.SetActive(data.IsBuy && !data.IsMaxLevel);
            _priceImage.gameObject.SetActive(!data.IsMaxLevel);
            _price.gameObject.SetActive(!data.IsMaxLevel);

            if (!data.IsAvailable && !data.IsBuy)
            {
                _workerImage.color = new Color(255, 255, 255, 0.5f);
            }
            else
            {
                _workerImage.color = new Color(255, 255, 255, 1);

            }

            if (!data.IsAvailable)
            {
                _priceImage.color = new Color(255, 255, 255, 0.5f);
                _price.color = new Color(_price.color.a, _price.color.g, _price.color.b, 0.5f);
                _status.color = new Color(255, 255, 255, 0.5f);
            }
            else
            {
                _priceImage.color = new Color(255, 255, 255, 1);
                _price.color = new Color(_price.color.a, _price.color.g, _price.color.b, 1f);
                _status.color = new Color(255, 255, 255, 1);
            }
        }

        public void BuyOrUpgradeButton()
        {
            Click?.Invoke(_name.text);
        }

        private void UpdateView(WorkersViewData data)
        {
            _price.text = data.Price;
            _level.text = data.Level;
            _status.text = data.IsBuy ? "Улучшить" : "Нанять";
            _nextBonusValue.text = data.NextBonusValue;
            _bonusValue.text = data.BonusValue;
        }
    }
}