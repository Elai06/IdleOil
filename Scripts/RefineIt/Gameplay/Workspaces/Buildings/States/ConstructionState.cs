using System;
using System.Collections;
using Gameplay.Tilemaps.Buildings;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Buildings.States
{
    public class ConstructionState : BuildingState
    {
        public event Action<float> OnChangeProgress;
        private readonly ICoroutineService _coroutineService;
        private readonly BuildingProgress _progress;
        private readonly BuildingStaticData _staticData;
        private readonly IBuilding _building;

        public ConstructionState(BuildingProgress progress, BuildingStaticData staticData, ICoroutineService coroutineService, IBuilding building)
        {
            _progress = progress;
            _staticData = staticData;
            _coroutineService = coroutineService;
            _building = building;
        }

        public float Progress => _progress.ConstructionProgress;

        public override void Enter()
        {
            _coroutineService.StartCoroutine(Building());
        }

        public override void Exit()
        {
            _building.Build();
        }

        private IEnumerator Building()
        {
            var currentProgress = Progress * _staticData.BuildTime;
            while(currentProgress < _staticData.BuildTime)
            {
                currentProgress += Time.deltaTime;
                _progress.ConstructionProgress = currentProgress / _staticData.BuildTime; 
                OnChangeProgress?.Invoke(_progress.ConstructionProgress);
                yield return null;
            }
            StateMachine.Enter<IdleState>();
        }
    }
}