using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class ProcessingView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        [SerializeField] private TMP_Text _description;
        [SerializeField] private TextMeshProUGUI _level;

        [SerializeField] private RectTransform _upgradeButton;
        [SerializeField] private RectTransform _buildedButton;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private IconDescriptionView _priceView;
        [SerializeField] private Image _timeFill;
        [SerializeField] private Image _timePause;

        [field: SerializeField]
        public SubViewContainer<ProcessingResourceSubView, ProcessingResourceData> ProduceSubViewContainer { get; set; }

        [field: SerializeField]
        public SubViewContainer<ProcessingResourceSubView, ProcessingResourceData> RequiredSubViewContainer { get; set; }
        
        [field: SerializeField]
        public SubViewContainer<IconDescriptionView, IconDescriptionData> ProduceConversionSubViewContainer { get; set; }

        [field: SerializeField]
        public SubViewContainer<IconDescriptionView, IconDescriptionData> RequiredConversionSubViewContainer { get; set; }

        public void ViewProcessingCost(Sprite sprite, string price)
        {
            _priceView.ViewSprite(sprite);
            _priceView.ViewValue(price);
        }

        public void ViewProcessingData(Sprite sprite, string description)
        {
            UpdateProcessingSprite(sprite);
            ViewComplexDescription(description);
        }

        private void ViewComplexDescription(string description)
        {
            _description.text = description;
        }

        public void UpdateProcessingSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void ViewBuilded(bool isBuild)
        {
            _buildedButton.gameObject.SetActive(!isBuild);
            _timePause.transform.parent.gameObject.SetActive(isBuild);
        }
        
        public void ViewUpgrade(bool upgradable)
        {
            _upgradeButton.gameObject.SetActive(upgradable);
        }

        public void PriceView(bool isActive)
        {
            _priceView.gameObject.SetActive(isActive);
        }

        public void ViewButtons(bool isActive)
        {
            _priceView.transform.parent.gameObject.SetActive(isActive);
        }

        public void TimeView(int time)
        {
            _timer.text = $"{time} сек";
        }

        public void SetLevelView(int level)
        {
            _level.text = $"Ур.{level}";
        }

        public void TimeFillView(float time)
        {
            _timeFill.fillAmount = time;
        }

        public void TimePause(bool isActive)
        {
            _timePause.gameObject.SetActive(isActive);
        }
    }
}