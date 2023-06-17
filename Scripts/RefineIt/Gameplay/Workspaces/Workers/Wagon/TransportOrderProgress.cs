using System;
using Gameplay.Workspaces.Workers.Transport;

namespace Gameplay.Workspaces.Workers.Wagon
{
    [Serializable]
    public class TransportOrderProgress 
    {
        public string Guid;
        public TransportState WagonState;

        public TransportOrderProgress(string guid)
        {
            Guid = guid;
        }
    }
}