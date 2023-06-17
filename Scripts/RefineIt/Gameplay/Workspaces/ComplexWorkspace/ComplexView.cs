using System.Collections.Generic;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class ComplexView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        [SerializeField] private TMP_Text _description;

        [SerializeField] private RectTransform _upgradeButton;
        [SerializeField] private RectTransform _buildedButton;
        [SerializeField] private RectTransform _shadowButton;

        [SerializeField] private IconDescriptionView _priceView;

        [field:SerializeField]public SubViewContainer<ComplexResourceSubView, ComplexResourceData> ResourceSubViews { get; set; }


        public void ViewComplexCost(Sprite sprite, string price)
        {
            _priceView.ViewSprite(sprite);
            _priceView.ViewValue(price);
        }

        public void UpdateCost(string price)
        {
            _priceView.ViewValue(price);
        }

        public void ViewComplexData(Sprite sprite, string description)
        {
            UpdateComplexSprite(sprite);
            ViewComplexDescription(description);
        }

        private void ViewComplexDescription(string description)
        {
            _description.text = description;
        }

        public void UpdateComplexSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void InitializeSubView(ComplexResourceData data)
        {
            ResourceSubViews.Add(data.ResourceType.ToString(), data);
        }

        public void ViewUpgrade(bool upgradable)
        {
            _upgradeButton.gameObject.SetActive(upgradable);
        }

        public void ViewBuilded(bool isBuilded)
        {
            _buildedButton.gameObject.SetActive(!isBuilded);
        }

        public void ViewPrice(bool isActive)
        {
            _priceView.gameObject.SetActive(isActive);
            _shadowButton.gameObject.SetActive(isActive);
        }
    }
}