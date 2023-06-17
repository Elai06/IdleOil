using DG.Tweening;
using UnityEngine;

namespace MVVMLibrary.Settings
{
    [CreateAssetMenu(fileName = "WidgetSettings", menuName = "Assets/AnimationSettings/WidgetSettings", order = 0)]
    public class WidgetsSettings : ScriptableObject
    {
        [SerializeField] private float _delayBetweenElements;
        [SerializeField] private float _elementAppearanceDuration;
        [SerializeField] private Ease _elementAppearanceEase;

        public float DelayBetweenElements => _delayBetweenElements;
        public float ElementAppearanceDuration => _elementAppearanceDuration;
        public Ease ElementAppearanceEase => _elementAppearanceEase;
    }
}