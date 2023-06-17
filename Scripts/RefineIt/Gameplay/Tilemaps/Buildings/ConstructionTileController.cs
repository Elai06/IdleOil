using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Services;
using Gameplay.Tilemaps.Views;
using Gameplay.Workspaces.Buildings.States;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Buildings
{
    public class ConstructionTileController
    {
        private readonly ITileStorage _tileStorage;
        private readonly BuildingTileView _view;
        private readonly RangeData[] _statesData;
        
        private int _currentStateIndex = -1;
        private ConstructionState _constructionState;

        public ConstructionTileController(ITileStorage tileStorage, BuildingTileView view)
        {
            _statesData = new[]
            {
                new RangeData(0f, 0f),
                new RangeData(0f, 0.5f),
                new RangeData(0.5f, 1f),
            };
            _view = view;
            _tileStorage = tileStorage;
        }

        public void View(ConstructionState constructionState)
        {
            _constructionState = constructionState;
            constructionState.OnChangeProgress += CheckAndViewNextState;
            CheckAndViewNextState(constructionState.Progress);
        }

        private void CheckAndViewNextState(float progress)
        {
            var state = GetCurrentState(progress);
            if(_currentStateIndex != state)
            {
                _currentStateIndex = state;
                ViewConstructionState(state);
                if(state == _statesData.Length - 1)
                    _constructionState.OnChangeProgress -= CheckAndViewNextState;
            }
        }

        private void ViewConstructionState(int currentState)
        {
            View(_tileStorage.GetConstructionTilesData(currentState));
        }

        private int GetCurrentState(float progress)
        {
            for(var i = 0; i < _statesData.Length; i++)
            {
                var rangeData = _statesData[i];
                if(rangeData.InRange(progress))
                    return i;
            }
            return _statesData.Length - 1;
        }

        private void View(AnimatedTileData animatedTileData)
        {
            var tileBase = animatedTileData.DefaultTile;
            _view.SetTile(tileBase);
            var animatedTile1 = animatedTileData.AnimatedTile1;
            var animatedTile2 = animatedTileData.AnimatedTile2;
            _view.SetAnimationTiles(animatedTile1, animatedTile2);
        }
    }
}