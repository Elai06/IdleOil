using UnityEngine;

namespace MVVMLibrary.Settings
{
    [CreateAssetMenu(fileName = "AnimationSettings", menuName = "Assets/AnimationSettings", order = 0)]
    public class AnimationSettings : ScriptableObject
    {
        [SerializeField] private PunchAnimationSettings _buttonClickAnimationSettings;
        [SerializeField] private PunchAnimationSettings _attractableAnimationSettings;
        [SerializeField] private WidgetsSettings _widgetsSettings;
        [SerializeField] private TooltipAnimationSettings _tooltipAnimationSettings;

        public PunchAnimationSettings ButtonClickAnimationSettings => _buttonClickAnimationSettings;
        public PunchAnimationSettings AttractableAnimationSettings => _attractableAnimationSettings;
        public WidgetsSettings WidgetsSettings => _widgetsSettings;
        public TooltipAnimationSettings TooltipAnimationSettings => _tooltipAnimationSettings;
    }
}