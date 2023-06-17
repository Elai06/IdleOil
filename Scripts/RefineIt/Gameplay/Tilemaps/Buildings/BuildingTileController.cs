using System;
using System.Collections.Generic;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Services;
using Gameplay.Tilemaps.Views;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.ComplexWorkspace;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Buildings
{
    public class BuildingTileController
    {
        private readonly IBuilding _building;
        private readonly BuildingTileView _view;
        private readonly ITileStorage _tileStorage;
        private readonly ConstructionTileController _constructionTileController;

        public BuildingTileController(IBuilding building, BuildingTileView view, ITileStorage tileStorage)
        {
            _view = view;
            _tileStorage = tileStorage;
            _building = building;
            _constructionTileController = new ConstructionTileController(tileStorage, view);
        }

        public void Initialize()
        {
            ViewState();
            ViewAdditionalComplexes();
        }

        public void Subscribe()
        {
            _building.OnStateChange += ViewState;
            if (_building is ILevelsBuilding levelsBuilding)
                levelsBuilding.LevelUpdated += ViewWorkingTile;
        }

        public void Unsubscribe()
        {
            _building.OnStateChange -= ViewState;
            if (_building is ILevelsBuilding levelsBuilding)
                levelsBuilding.LevelUpdated -= ViewWorkingTile;
        }

        public void Click()
        {
            _building.OnClick();
        }

        private void ViewFoundationTile() =>
            ViewWithAnimation(_tileStorage.GetFoundationTilesData());

        private void ViewState()
        {
            switch (_building.CurrentState)
            {
                case NotBuildedState notBuildedState:
                    ViewFoundationTile();
                    break;
                case ConstructionState constructionState:
                    ViewConstructionTile(constructionState);
                    break;
                case WorkingState workingState:
                    ViewWorkingTile();
                    break;
                case IdleState idleState:
                    ViewIdleState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpexted State for View {_building.CurrentState}");
            }
        }

        private void ViewConstructionTile(ConstructionState constructionState)
        {
            _constructionTileController.View(constructionState);
        }

        private void ViewWorkingTile()
        {
            if (_building is ILevelsBuilding levelsBuilding)
                ViewWorkingTile(levelsBuilding);
            else
            {
                ViewWithAnimation(GetLevelData());
            }
        }

        private void ViewWorkingTile(ILevelsBuilding levelsBuilding)
        {
            var level = levelsBuilding.CurrentLevel;
            if (_tileStorage.HasTiles(_building.Id, level) == false)
                return;

            if (ViewAdditionalComplex(levelsBuilding))
                return;

            ViewWithAnimation(GetLevelData(level));
        }

        private bool ViewAdditionalComplex(ILevelsBuilding levelsBuilding)
        {
            var complexTileData = _tileStorage.TileMapData
                .BuildingsData?.Find(x => x?.Position == _building.Guid);
            if (complexTileData?.Id == _building.Id && complexTileData?.AdditionalBuildings.Count > 0)
            {
                var numbersBuildings = levelsBuilding.NumbersBuildings > 0 ? levelsBuilding.NumbersBuildings - 1 : 0;
                
                ViewWithAnimation(GetLevelData(levelsBuilding.CurrentLevel),
                    complexTileData.AdditionalBuildings[numbersBuildings].Position);
                _view.DisableAnimation();

                return true;
            }

            return false;
        }

        private void ViewIdleState()
        {
            var level = _building is ILevelsBuilding levelsBuilding ? levelsBuilding.CurrentLevel : 0;
            if (_tileStorage.HasTiles(_building.Id, level) == false)
                return;
            View(GetLevelData(level));
        }

        private AnimatedTileData GetLevelData(int level = 0) =>
            _tileStorage.GetBuildingTilesData(_building.Id, level);


        private void View(AnimatedTileData animatedTileData)
        {
            var tileBase = animatedTileData.DefaultTile;
            _view.SetTile(tileBase);
            _view.DisableAnimation();
        }

        private void ViewWithAnimation(AnimatedTileData animatedTileData)
        {
            var tileBase = animatedTileData.DefaultTile;
            _view.SetTile(tileBase);
            var animatedTile1 = animatedTileData.AnimatedTile1;
            var animatedTile2 = animatedTileData.AnimatedTile2;
            _view.SetAnimationTiles(animatedTile1, animatedTile2);
        }

        private void ViewWithAnimation(AnimatedTileData animatedTileData, Vector3Int position)
        {
            var tileBase = animatedTileData.DefaultTile;
            _view.SetTile(tileBase, position);
            var animatedTile1 = animatedTileData.AnimatedTile1;
            var animatedTile2 = animatedTileData.AnimatedTile2;
            _view.SetAnimationTiles(animatedTile1, animatedTile2);
        }

        private void ViewAdditionalComplexes()
        {
            var complexTileData = _tileStorage.TileMapData.BuildingsData
                .Find(x => x.Position == _building.Guid);

            if (_building is ILevelsBuilding levelsBuilding && _building.IsBuilded)
                if (complexTileData.Id == _building.Id)
                {
                    for (var i = 0; i < levelsBuilding.NumbersBuildings; i++)
                    {
                        if (levelsBuilding.NumbersBuildings - 1 > i)
                        {
                            ViewWithAnimation(GetLevelData(_tileStorage.BuildingTiles[complexTileData.Id]
                                .Levels.Length - 1), complexTileData.AdditionalBuildings[i].Position);

                            _view.DisableAnimation();
                            continue;
                        }

                        ViewWithAnimation(GetLevelData(levelsBuilding.CurrentLevel),
                            complexTileData.AdditionalBuildings[i].Position);

                        _view.DisableAnimation();
                    }
                }
        }
    }
}