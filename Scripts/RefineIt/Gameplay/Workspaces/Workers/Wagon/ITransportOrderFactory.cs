using System.Threading.Tasks;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public interface ITransportOrderFactory
    {
        Task<TransportOrderModel> Create(TransportOrderPathMarker transportOrderPathMarker, OrderModel orderModel);

        void Update(TransportOrderPathMarker transportOrderPathMarker, OrderModel orderModel,
            TransportOrderModel transportOrderModels);
    }
}