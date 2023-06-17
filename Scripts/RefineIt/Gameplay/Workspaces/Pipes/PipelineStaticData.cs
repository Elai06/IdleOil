using System;
using System.Collections.Generic;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Pipes
{
    [CreateAssetMenu(fileName = "PipelineStaticData", menuName = "Data/PipelineData")]
    [Serializable]
    public class PipelineStaticData : ScriptableObject
    {
        public TransportType TransportType;
        public int SpeedTransfer;
        public int AmountResources;
        public List<ResourceType> Resources;
    }
}