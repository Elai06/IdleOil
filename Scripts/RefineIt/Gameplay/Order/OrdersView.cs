using Gameplay.Order;
using Gameplay.Orders;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.MVVM.Views
{
    public class OrdersView : MonoBehaviour
    {
        [field:SerializeField]  public SubViewContainer<OrderSubView, OrderModel> OrderSubView{ get; private set; }
    }
}