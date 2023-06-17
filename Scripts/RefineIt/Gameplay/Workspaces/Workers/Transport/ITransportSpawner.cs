using System.Collections.Generic;

namespace Gameplay.Workspaces.Workers.Transport
{
    public interface ITransportSpawner
    {
        void Initialize();
        IEnumerable<TransportModel> Transports { get; }
    }
}