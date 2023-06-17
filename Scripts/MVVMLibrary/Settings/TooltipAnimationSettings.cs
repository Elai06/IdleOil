using DG.Tweening;
using UnityEngine;

namespace MVVMLibrary.Settings
{
    [CreateAssetMenu(fileName = "TooltipAnimationSettings", menuName = "Assets/TooltipAnimationSettings", order = 0)]
    public class TooltipAnimationSettings : ScriptableObject
    {
        public static string Path = "TooltipAnimationSettings";

        [Header("Main Settings")]
        [SerializeField] private float _duration;
        [SerializeField] private float _hideDelay = 3f;

        [Header("Easing ")]
        [SerializeField] private Ease _easing;

        public float Duration => _duration;
        public Ease Easing => _easing;
        public float HideDelay => _hideDelay;
    }
}