using System;
using Gameplay.Workspaces.ComplexWorkspace;

namespace Gameplay.Workspaces.Workers.Transport
{
    [Serializable]
    public class TransportLevelData
    {
        public ResourceCapacity[] Capacities;
        public float ShippingTime;
        public float ExportTime;
        public float ImportTime;
        public float ReturnTime;
    }
}