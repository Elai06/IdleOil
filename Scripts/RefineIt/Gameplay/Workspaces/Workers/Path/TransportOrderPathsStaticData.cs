using System.Collections.Generic;
using Gameplay.Tilemaps;
using Gameplay.Workspaces.Workers.Wagon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Path
{
    [CreateAssetMenu(fileName = "TransportOrderPathsData", menuName = "Data/transportOrderPaths")]
    public class TransportOrderPathsStaticData : ScriptableObject
    {
        public List<TransportOrderPathData> Paths;
#if UNITY_EDITOR
        [Button]
        public void Collect()
        {
            var tilemapMarker = FindObjectOfType<TilemapMarker>();
            var pathMarkers = FindObjectsOfType<TransportOrderPathMarker>();
            Paths.Clear();
            foreach (var pathMarker in pathMarkers)
            {
                var importGuid = tilemapMarker.InteractableTilemap.WorldToCell(pathMarker.StartPoint.position);
                var exportGuid = tilemapMarker.InteractableTilemap.WorldToCell(pathMarker.EndPoint.position);
                Paths.Add(new TransportOrderPathData(pathMarker.Guid, importGuid, exportGuid, pathMarker.transportOrderType));
            }
        }
#endif
    }
}