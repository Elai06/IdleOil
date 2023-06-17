using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.Shop
{
    public class ShopView : MonoBehaviour
    {
        [field: SerializeField]
        public SubViewContainer<PurchaseSubView, PurchaseViewData> SetProductSubViewContainer { get; private set; }

        [field: SerializeField]
        public SubViewContainer<PurchaseSubView, PurchaseViewData> HardProductSubViewContainer { get; private set; }

        [field: SerializeField]
        public SubViewContainer<CurrencyProductSubView, CurrencyViewData> WorkerCurrencySubViewContainer { get; private set; }

        [field: SerializeField]
        public SubViewContainer<CurrencyProductSubView, CurrencyViewData> SoftProductSubViewContainer { get; private set; }
    }
}