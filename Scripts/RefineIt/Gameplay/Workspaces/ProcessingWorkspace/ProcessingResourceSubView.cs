using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class ProcessingResourceSubView : SubView<ProcessingResourceData>
    {
       
        [SerializeField] private Image _image;
       // [SerializeField] private TextMeshProUGUI _resourceName;
        [SerializeField] private TextMeshProUGUI _resourceSliderValue;
        [SerializeField] private TextMeshProUGUI _nextLevelResourceCapacity;
        [SerializeField] private Slider _slider;

        private int _capacity;

        public void ViewUpdateCapacity(int value, int capacity)
        {
            _resourceSliderValue.text = $"{value}/{capacity}";
            _slider.maxValue = capacity;
            _slider.value = value;
            _capacity = capacity;
        }

        public override void Initialize(ProcessingResourceData data)
        {
            _capacity = data.ResourceCapacity;
            _image.sprite = data.ResourceSprite;
           // _resourceName.text = data.ResourceType.ToString();
            _nextLevelResourceCapacity.text = data.NextLevelResourceCapacity.ToString();
            _nextLevelResourceCapacity.gameObject.SetActive(data.IsBuilded);

            SliderValueView(data);
        }
        
        public void SliderValueView(ProcessingResourceData data)
        {
            _slider.maxValue = data.ResourceCapacity;
            _slider.value = data.Value;
            _resourceSliderValue.text = $"{data.Value}/{data.ResourceCapacity}";
        }

        public void UpdateSliderValue(int value)
        {
            _resourceSliderValue.text = $"{value}/{_capacity}";
            _slider.value = value;
        }
    }
}