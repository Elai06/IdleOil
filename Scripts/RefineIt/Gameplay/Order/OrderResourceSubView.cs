using Gameplay.Orders;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.MVVM.Views
{
    public class OrderResourceSubView : SubView<OrderResourceData>
    {
        [SerializeField] private TextMeshProUGUI _resourceAmountField;
        [SerializeField] private Image _resourceImage;

        public override void Initialize(OrderResourceData data)
        {
            _resourceImage.sprite = data.ResourceSprite;
            _resourceAmountField.text = data.Quantity.ToString();
        }
    }
}