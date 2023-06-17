using System;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Shop
{
    public class CurrencyProductSubView : SubView<CurrencyViewData>
    {
        public event Action Click;

        [SerializeField] private Image _purchaseImage;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _productValue;
        [SerializeField] private TextMeshProUGUI _productName;

        public override void Initialize(CurrencyViewData data)
        {
            SetProductValue(data);

            _purchaseImage.sprite = data.PurchaseSprite;
            _currencyImage.sprite = data.CostSprite;
            _priceValue.text = data.PurchaseProductData.CostProduct.Value.ToString();
            _productName.text = data.PurchaseProductData.Name;
        }

        private void SetProductValue(CurrencyViewData viewData)
        {
            if (viewData.PurchaseProductData.ProductType != ProductType.Set)
            {
                _productValue.text = viewData.PurchaseProductData.PurchaseProduct.Value.ToString();
            }
        }

        public void BuyClick()
        {
            Click?.Invoke();
        }
    }
}