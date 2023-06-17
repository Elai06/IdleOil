using System.Collections.Generic;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public interface ITransportOrderSpawner
    {
        void Initialize();
        IEnumerable<TransportOrderModel> Wagons { get; }
    }
}