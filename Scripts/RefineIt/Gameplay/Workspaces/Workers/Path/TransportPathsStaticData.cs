using System.Collections.Generic;
using Gameplay.Tilemaps;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Gameplay.Workspaces.Workers.Path
{
    [CreateAssetMenu(fileName = "TransportPathsData", menuName = "Data/TransportPaths")]
    public class TransportPathsStaticData : ScriptableObject
    {
        public List<TransportPathData> Paths;
#if UNITY_EDITOR
        [Button]
        public void Collect()
        {
            var tilemapMarker = FindObjectOfType<TilemapMarker>();
            var pathMarkers = FindObjectsOfType<TransportPathMarker>();
            Paths.Clear();
            foreach (var pathMarker in pathMarkers)
            {
                var importGuid = tilemapMarker.InteractableTilemap.WorldToCell(pathMarker.ImportPoint.position);
                var exportGuid = tilemapMarker.InteractableTilemap.WorldToCell(pathMarker.ExportPoint.position);
                Paths.Add(new TransportPathData(pathMarker.Guid, importGuid, exportGuid, pathMarker.TransportType));
            }
        }
#endif
    }
}