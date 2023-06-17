using System;
using Gameplay.Region;

namespace Gameplay.Workspaces.Workers.Transport
{
    [Serializable]
    public class TransportProgress
    {
        public string Guid;
        public TransportType TransportType;
        public TransportState CurrentState = TransportState.Return;
        public int CurrentLevel;

        public StorageProgress StorageProgress;

        public TransportProgress(string guid, TransportType transportType)
        {
            Guid = guid;
            TransportType = transportType;
            StorageProgress = new StorageProgress();
        }
    }
}