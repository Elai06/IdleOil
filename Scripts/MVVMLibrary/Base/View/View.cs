using System.Collections.Generic;
using DG.Tweening;
using MVVMLibrary.Enums;
using MVVMLibrary.Settings;
using UniRx;
using UnityEngine;
using Utils;

namespace MVVMLibrary.Base.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class View<T> : MonoBehaviour, IView where T : ViewModel.ViewModel
    {
        [SerializeField] private bool _isInteractable = true;
        
        private Dictionary<Transform, Sequence> _animationObjects = new Dictionary<Transform, Sequence>();
        private AnimationSettings _animationSettings;

        protected CanvasGroup CanvasGroup;
        protected T ViewModel;
        protected CompositeDisposable Subscribtions = new CompositeDisposable();

        public abstract Layer EUiLayer { get; }
        
        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            if (ViewModel == null) return;
            
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        public virtual void Subscribe()
        {
            ViewModel.AddSubscriptions();
        }

        public virtual void Unsubscribe()
        {
            Subscribtions.Clear();
            ViewModel.RemoveSubscriptions();
        }

        public void SetViewModel<T1>(T1 viewModel) where T1 : ViewModel.ViewModel
        {
            ViewModel = viewModel as T;
            ViewModel?.SetData();
        }

        public virtual void Show()
        {
            if (CanvasGroup == null)
            {
                this.LogError($"View {GetType()} has no CanvasGroup \n " +
                                           $"need to initialize view before show");
            }
            
            this.Log($"{name} Show");
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = _isInteractable;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.Log($"{name} Hide");
            //CanvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
        
        public void SetViewParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}