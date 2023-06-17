using Gameplay.Orders;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.MVVM.Views
{
    public class OrderRewardSubView : SubView<OrderRewardData>
    {
        [SerializeField] private TextMeshProUGUI _resourceAmountField;
        [SerializeField] private Image _resourceImage;
        public override void Initialize(OrderRewardData data)
        {
            _resourceImage.sprite = data.RewardSprite;
            _resourceAmountField.text = data.RewardAmount.ToString();
        }
    }
}