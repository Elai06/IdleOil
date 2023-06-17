using System;
using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Path
{
    [Serializable]
    public class TransportPathData
    {
        public string Guid;
        public Vector3Int StartPoint;
        public Vector3Int EndPoint;
        public TransportType TransportType;

        public TransportPathData(string guid, Vector3Int startPoint, Vector3Int endPoint, TransportType transportType)
        {
            Guid = guid;
            StartPoint = startPoint;
            EndPoint = endPoint;
            TransportType = transportType;
        }
    }
}