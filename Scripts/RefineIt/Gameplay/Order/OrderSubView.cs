using System;
using System.Collections.Generic;
using Gameplay.MVVM.Views;
using Gameplay.Orders;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Order
{
    public class OrderSubView : SubView<OrderModel>
    {
        private const string ORDER = "Заказ ";
        [SerializeField] private TextMeshProUGUI _orderNumber;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _timerField;
        [SerializeField] private GameObject _timer;
        [SerializeField] private Image _avatar;

        [SerializeField] private SubViewContainer<OrderResourceSubView, OrderResourceData> _resourceSubViewContainer;
        [SerializeField] private SubViewContainer<OrderRewardSubView, OrderRewardData> _rewardSubViewContainer;

        [SerializeField] private GetRewardOrderButton _getRewardOrderButton;
        [SerializeField] private TakeOrderButton _takeOrderButton;
        [SerializeField] private ReplaceOrderButton _adsOrderButton;

        public override void Initialize(OrderModel model)
        {
            _orderNumber.text = $"{ORDER} {model.ID + 1}";
            _description.text = OrderDescription.GetOrderDescription(model);
            _avatar.sprite = model.ClientSprite;

            AddResourceSubViews(model.ResourcesData);
            AddRewardSubViews(model.RewardsData);

            _getRewardOrderButton.Initialize(model);
            _takeOrderButton.Initialize(model);
            _adsOrderButton.Initialize(model);  
            _timer.SetActive(model.IsTimer);

            UpdateViewButton(model);
        }

        public void SetTimer(string timer)
        {
            _timerField.text = timer;
        }

        private void UpdateViewButton(OrderModel model)
        {
            _takeOrderButton.gameObject.SetActive(false);
            _getRewardOrderButton.gameObject.SetActive(false);
            _adsOrderButton.gameObject.SetActive(model.OrderProgress.isCanShowAds);

            switch (model.OrderProgress.OrderStatus)
            {
                case OrderStatus.Idle:
                    _takeOrderButton.gameObject.SetActive(true);
                    break;
                case OrderStatus.AvailableReward:
                    _takeOrderButton.gameObject.SetActive(false);
                    _getRewardOrderButton.gameObject.SetActive(true);
                    break;
                case OrderStatus.Working:
                    break;
                case OrderStatus.Complete:
                    break;
                case OrderStatus.Failed:
                    break;
                case OrderStatus.AdsReplace:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddResourceSubViews(List<OrderResourceData> _orderResourceData)
        {
            _resourceSubViewContainer.CleanUp();

            foreach (var resource in _orderResourceData)
            {
                _resourceSubViewContainer.Add(resource.ResourceType.ToString(), resource);
            }
        }

        private void AddRewardSubViews(OrderRewardData _orderRewardData)
        {
            _rewardSubViewContainer.CleanUp();

            _rewardSubViewContainer.Add(_orderRewardData.RewardType.ToString(), _orderRewardData);
        }

        public void SetAdsButton(bool isActive)
        {
            _adsOrderButton.gameObject.SetActive(isActive);
        }
    }
}