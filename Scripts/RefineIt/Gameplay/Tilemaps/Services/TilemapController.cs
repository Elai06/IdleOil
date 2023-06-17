using System;
using System.Collections.Generic;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Factories;
using Gameplay.Tilemaps.Views;
using UnityEngine;

namespace Gameplay.Tilemaps.Services
{
    public class TilemapController : ITilemapController
    {
        public event Action<Vector3Int> ClickOnBuild;

        private readonly Dictionary<Vector3Int, BuildingTileController> _buildings = new();

        private readonly ITileClickHandler _tilemapClickHandler;
        private readonly ITileViewFactory _tileViewFactory;
        private readonly ITileStorage _tileStorage;

        public TilemapController(ITileClickHandler tilemapClickHandler, ITileViewFactory tileViewFactory,
            ITileStorage tileStorage)
        {
            _tilemapClickHandler = tilemapClickHandler;
            _tileViewFactory = tileViewFactory;
            _tileStorage = tileStorage;
        }

        public void Initialize()
        {
            _tilemapClickHandler.Clicked += OnClick;
        }

        public void Add(IBuilding building)
        {
            BuildingTileView buildingTileView = _tileViewFactory.Create(building.Guid);
            BuildingTileController buildingTileController = new(building, buildingTileView, _tileStorage);
            buildingTileController.Initialize();
            buildingTileController.Subscribe();
            _buildings.Add(building.Guid, buildingTileController);
        }

        public void Cleanup()
        {
            foreach (BuildingTileController buildingTileController in _buildings.Values)
                buildingTileController.Unsubscribe();
            _buildings.Clear();
            _tilemapClickHandler.Clicked -= OnClick;
        }

        private void OnClick(Vector3Int position)
        {
            if (_buildings.TryGetValue(position, out BuildingTileController buildingTileController))
            {
                buildingTileController.Click();
                ClickOnBuild?.Invoke(position);
            }
        }

        private void ClickAdditionalBuilding(Vector3Int position)
        {
            foreach (BuildingData buildingData in _tileStorage.TileMapData.BuildingsData)
            {
                AdditionalBuilding additionalBuilding =
                    buildingData.AdditionalBuildings?.Find(x => x?.Position == position);
                if (additionalBuilding?.Position == position)
                {
                    if (_buildings.TryGetValue(buildingData.Position,
                            out BuildingTileController additionalBuildingTileController))
                        additionalBuildingTileController.Click();
                }
            }
        }
    }
}