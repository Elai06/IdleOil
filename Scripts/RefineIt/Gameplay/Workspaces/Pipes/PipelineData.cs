using System;
using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Pipes
{
    [Serializable]
    public class PipelineData
    {
        public string Guid;
        public Vector3Int ImportBuildGuid;
        public Vector3Int ExportBuildGuid;
        public TransportType TransportType;
        public float Duration;

        public PipelineData(string guid, Vector3Int importBuildGuid, Vector3Int exportBuildGuid, float duration,
            TransportType transportType)
        {
            Guid = guid;
            ImportBuildGuid = importBuildGuid;
            ExportBuildGuid = exportBuildGuid;
            Duration = duration;
            TransportType = transportType;
        }
    }
}