using System;
using Gameplay.Workspaces.Workers.Wagon;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Path
{
    [Serializable]
    public class TransportOrderPathData
    {
        public string Guid;
        public Vector3Int ImportPoint;
        public Vector3Int ExportPoint;
        public TransportOrderType transportOrderType;

        public TransportOrderPathData(string guid, Vector3Int importPoint, Vector3Int exportPoint, TransportOrderType transportType)
        {
            Guid = guid;
            ImportPoint = importPoint;
            ExportPoint = exportPoint;
            transportOrderType = transportType;
        }
    }
}