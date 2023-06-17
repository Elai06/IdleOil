using System;
using System.Collections.Generic;
using MVVMLibrary.Base.LayoutWidget;
using UnityEngine;

namespace MVVMLibrary.Base.Widget
{
    public interface IWidget<TData, TCallback>
    {
        public event Action<TCallback> Selected;

        LayoutWidgetElement<TData> LayoutWidgetElementPrefab { get; }
        RectTransform Container { get; }

        public TData[] ElementsData { get; }

        public void Initialize(IList<TData> tabsData);
        public void SetTabData(int index, TData tabData);
        public void Clear();
        public void SetSelectableForElement(int elementIndex, bool isSelectable);
    }
}