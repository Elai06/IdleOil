using System.Collections.Generic;
using Gameplay.Tilemaps;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Gameplay.Workspaces.Pipes
{
    [CreateAssetMenu(fileName = "PipelinesTilemapData", menuName = "Data/Pipelines")]
    public class PipelinesTilemapStaticData : ScriptableObject
    {
        public List<PipelineData> PipelinesData;

#if UNITY_EDITOR
        [Button]
        public void Collect()
        {
            PipelinesData.Clear();

            var tilemapMarker = FindObjectOfType<TilemapMarker>();
            var pathMarkers = FindObjectsOfType<PipelineMarker>();
            foreach (var pathMarker in pathMarkers)
            {
                var importGuid = tilemapMarker.InteractableTilemap.WorldToCell(pathMarker.ImportPoint.position);
                var exportGuid = tilemapMarker.InteractableTilemap.WorldToCell(pathMarker.ExportPoint.position);
                PipelinesData.Add(new PipelineData(pathMarker.Guid, importGuid, exportGuid, pathMarker.Duration, pathMarker.TransportType));
            }
        }
#endif
    }
}