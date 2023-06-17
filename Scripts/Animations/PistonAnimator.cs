using DG.Tweening;
using UnityEngine;

namespace RefineIt
{
    public class PistonAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _piston;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private Tween _animation;

        private void OnEnable()
        {
            _piston.localPosition = Vector3.zero;
            _animation = _piston.DOLocalMove(Vector3.up, _duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(_ease);
        }

        private void OnDisable()
        {
            _animation?.Kill();
        }
    }
}
