using System;
using DG.Tweening;
using MVVMLibrary.Settings;
using UnityEngine;
using Utils;
using Zenject;

namespace MVVMLibrary.Base.LayoutWidgetElement
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class LayoutWidgetElement<T> : MonoBehaviour
    {
        public event Action<LayoutWidgetElement<T>> Selected;
        
        [SerializeField] private RectTransform _container;
        [SerializeField] private bool _isAnimatedOnEnable;
        [SerializeField] private Vector3 _offset;
        
        private Tween _visibilityTween;
        private WidgetsSettings _animationSettings;
        private CanvasGroup _canvasGroup;
        
        public RectTransform RectTransform { get; protected set; }
        public bool IsSelectable { get; protected set; } = true;
        public int Index { get; private set; }
        public T Data { get; private set; }
        protected CanvasGroup CanvasGroup => _canvasGroup;
        private float Duration => _animationSettings.ElementAppearanceDuration;
        
        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings.WidgetsSettings;
        }
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();

            if (_container == null)
            {
                _container = GetComponent<RectTransform>();
                this.LogWarning($"{name} has no container", caller:nameof(gameObject));
            }

            OffsetPosition();
        }
        
        protected virtual void OnEnable()
        {
            if (_isAnimatedOnEnable)
            {
                AppearanceAnimation();
            }
        }

        protected virtual void OnDisable()
        {
            _visibilityTween?.Kill();
            LerpVisuals(1);
            
            OffsetPosition();
        }
        
        public virtual void SetData(T data)
        {
            Data = data;
        }

        public virtual void OnSelection()
        {
            if (IsSelectable)
            {
                Selected?.Invoke(this);
            }
        }
        
        public void SetIndex(int index)
        {
            Index = index;
        }

        public virtual void SetSelectable(bool isSelectable)
        {
            IsSelectable = isSelectable;
        }

        private void OffsetPosition()
        {
            if (_isAnimatedOnEnable)
            {
                _container.transform.localPosition = Vector3.up * 100;
            }
        }

        public virtual void AppearanceAnimation()
        {
            _visibilityTween?.Kill();
            _visibilityTween = DOVirtual.Float(0, 1, Duration, LerpVisuals)
               .SetEase(_animationSettings.ElementAppearanceEase);
        }

        public void DisappearanceAnimation()
        {
            _visibilityTween?.Kill();
            _visibilityTween = DOVirtual.Float(1, 0, Duration, LerpVisuals)
                .SetEase(_animationSettings.ElementAppearanceEase);
        }

        public virtual void LerpVisuals(float step)
        {
            _canvasGroup.alpha = step;
            _container.localPosition = Vector3.Lerp(_offset, Vector3.zero, step);
        }
    }
}