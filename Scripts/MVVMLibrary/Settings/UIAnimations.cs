using DG.Tweening;
using ModestTree;
using UnityEngine;

namespace MVVMLibrary.Settings
{
    public static class UIAnimations
    {
        private static Tween _fadeTween;
        private static Sequence _clickSequence;

        public static PunchAnimationSettings Settings { get; private set; }

        public static void SetPunchAnimationSettings(PunchAnimationSettings settings)
        {
            Settings = settings;
        }

        public static void EaseClickAnimation(Transform transform)
        {
            if (transform == null)
            {
                Log.Error($"This transform {transform.name} is null");
                return;
            }

            transform.DOKill();
            transform.DOScale(Settings.PunchClick, Settings.EaseClickDuration).SetLoops(2, LoopType.Yoyo);
        }

        public static void FadeAnimation(CanvasGroup canvas)
        {
            if (canvas == null)
            {
                Log.Error($"this canvas {canvas} is null");
                return;
            }

            canvas.alpha = 0;

            _fadeTween?.Kill();
            _fadeTween = DOVirtual.Float(0, 1, Settings.Duration, value => canvas.alpha = value);
        }

        public static void TwitchingAnimation(Transform transform)
        {
            if (transform == null)
            {
                Log.Error($"This transform {transform.name} is null");
            }

            transform.eulerAngles = new Vector3(0, 0, -Settings.TwitchPunch);
            transform.DOLocalRotate(new Vector3(0, 0, Settings.TwitchPunch), Settings.TwitchDuration).SetLoops(-1, LoopType.Yoyo);
        }

        public static void StopAnimation(Transform transform)
        {
            if (transform == null)
            {
                Log.Error($"This transform {transform.name} is null");
            }

            transform.DOKill();
        }
    }
}