using System;
using UnityEngine;

namespace Gameplay.Workspaces.Pipes
{
    [Serializable]
    public class PipeData
    {
        public string PipeTileId;
        public Vector3Int Position;

        public PipeData(string pipeTileId, Vector3Int position)
        {
            PipeTileId = pipeTileId;
            Position = position;
        }
    }
}