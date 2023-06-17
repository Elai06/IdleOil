using System;
using DG.Tweening;
using UnityEngine;

namespace MVVMLibrary.Base.LayoutWidget
{
    public abstract class LayoutWidgetElement<T> : MonoBehaviour
    {
        public event Action<LayoutWidgetElement<T>> Selected;
        
        private Tween _visibilityTween;

        public bool IsSelectable { get; protected set; } = true;
        public int Index { get; private set; }
        public T Data { get; private set; }
        
        protected virtual void OnDisable()
        {
            _visibilityTween?.Kill();
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
    }
}