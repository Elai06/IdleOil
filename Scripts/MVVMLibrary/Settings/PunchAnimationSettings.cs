using DG.Tweening;
using UnityEngine;

namespace MVVMLibrary.Settings
{
    [CreateAssetMenu(fileName = "PunchAnimationSettings", menuName = "Assets/AnimationSettings/PunchAnimationSettings",
        order = 0)]
    public class PunchAnimationSettings : ScriptableObject
    {
        public static string Path = "PunchAnimationSettings";

        [Header("Main Settings")] 
        [SerializeField] private float _duration;
        [SerializeField] private float _easeClickDuration;
        [SerializeField] private float _punchClick;
        [SerializeField] private float _twitchPunch;
        [SerializeField] private float _twitchDuration;
        [Range(0f, 1f)] [SerializeField] private float _elasticity;
        [Header("Easing ")]
        [SerializeField] private Ease _easing;
        [SerializeField] private Ease _twitchEasing;

        public float Duration => _duration;
        public Ease Easing => _easing;

        public float TwitchPunch => _twitchPunch;

        public float TwitchDuration => _twitchDuration;

        public float EaseClickDuration => _easeClickDuration;

        public float PunchClick => _punchClick;

        public Ease TwitchEasing => _twitchEasing;
    }
}