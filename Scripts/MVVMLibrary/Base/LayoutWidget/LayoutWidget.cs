using System;
using System.Collections.Generic;
using System.Linq;
using MVVMLibrary.Base.Widget;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace MVVMLibrary.Base.LayoutWidget
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class LayoutWidget<TData, TCallback> : MonoBehaviour, IWidget<TData, TCallback>
        where TData : IEquatable<TData>
    {
        public event Action<TCallback> Selected;

        [SerializeField] protected LayoutWidgetElement<TData> _layoutWidgetElementPrefab;
        [SerializeField] protected RectTransform _tabContainer;

        protected readonly List<LayoutWidgetElement<TData>> Elements = new List<LayoutWidgetElement<TData>>();
        protected IInstantiateSpawner _injectSpawner;

        public LayoutWidgetElement<TData> LayoutWidgetElementPrefab => _layoutWidgetElementPrefab;
        public RectTransform Container => _tabContainer;

        public int ElementsCount => Elements.Count;
        public TData[] ElementsData => Elements.Select(x => x.Data).ToArray();

        [Inject]
        private void Construct(IInstantiateSpawner injectSpawner)
        {
            _injectSpawner = injectSpawner;
        }

        protected virtual void Awake()
        {
            if (_tabContainer == null)
            {
                _tabContainer = gameObject.GetComponent<RectTransform>();
            }
        }

        private void OnDestroy()
        {
            Clear();
        }

        public void Initialize(IList<TData> tabsData)
        {
            if (tabsData == null)
            {
                return;
            }

            for (var i = 0; i < tabsData.Count; i++)
            {
                if (i < Elements.Count)
                {
                    SetTabData(i, tabsData[i]);
                }
                else
                {
                    AddTab(tabsData[i]);
                }
            }
        }

        public virtual void AddTab(TData tabData)
        {
            var tab = _injectSpawner.Instantiate<LayoutWidgetElement<TData>>(_layoutWidgetElementPrefab.gameObject,
                _tabContainer);
            tab.SetIndex(Elements.Count);
            tab.SetData(tabData);


            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

            Elements.Add(tab);
            tab.Selected += OnTabSelected;
        }

        public void SetTabData(int index, TData tabData)
        {
            if (Elements.Count <= index)
            {
                for (var i = 0; i <= index; i++)
                {
                    AddTab(tabData);
                }
            }
            else
            {
                Elements[index].SetData(tabData);
            }
        }

        public void SetTabData(TData tabData)
        {
            Elements.FirstOrDefault(x => x.Data.Equals(tabData))?.SetData(tabData);
        }

        public virtual void Clear()
        {
            for (var i = 0; i < Elements.Count; i++)
            {
                Elements[i].Selected -= OnTabSelected;
                Destroy(Elements[i].gameObject);
            }

            Elements.Clear();
        }

        public void SetSelectable(TData tabData, bool isSelectable)
        {
            Elements.FirstOrDefault(x => x.Data.Equals(tabData))?.SetSelectable(isSelectable);
        }

        public void SetSelectableForElement(int elementIndex, bool isSelectable)
        {
            if (elementIndex < 0 || elementIndex >= Elements.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            Elements[elementIndex].SetSelectable(isSelectable);
        }

        protected abstract void OnTabSelected(LayoutWidgetElement<TData> layoutWidgetElement);

        protected virtual void OnSelected(TCallback obj)
        {
            Selected?.Invoke(obj);
        }
    }
}