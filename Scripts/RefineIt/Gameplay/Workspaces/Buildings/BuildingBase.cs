using System;
using Gameplay.Currencies;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Workspaces.Buildings.States;
using UnityEngine;
using UnityEngine.AddressableAssets;
using StateMachine = Infrastructure.StateMachine.StateMachine;

namespace Gameplay.Workspaces.Buildings
{
    public abstract class BuildingBase<TProgress, TData> : IBuilding
        where TProgress : BuildingProgress
        where TData : BuildingStaticData
    {
        private StateMachine _stateMachine;
        private readonly CurrenciesModel _currenciesModel;

        protected BuildingBase(TProgress progress, TData data, CurrenciesModel currenciesModel)
        {
            Progress = progress;
            Data = data;
            _currenciesModel = currenciesModel;
        }

        public event Action<IBuilding> Builded;

        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public event Action OnStateChange
        {
            add => _stateMachine.OnStateChange += value;
            remove => _stateMachine.OnStateChange -= value;
        }

        public AssetReferenceSprite SpriteView => Data.SpriteView;
        public BuildingState CurrentState => _stateMachine.CurrentState as BuildingState;
        public Vector3Int Guid => Progress.Guid;
        protected TData Data { get; }
        protected TProgress Progress { get; }
        public abstract string Id { get; }
        public int Cost => Data.Cost;
        public bool IsBuilded => Progress.IsBuilded;
        public bool CanBuy => _currenciesModel.Has(CostType, Cost);
        public CurrencyType CostType => Data.CostType;
        public bool IsConstructing => Progress.ConstructionProgress is > 0f and < 1f;
        
        public void Build()
        {
            Progress.IsBuilded = true;
            Builded?.Invoke(this);
        }

        public void OnClick()
        {
            CurrentState.OnClick();
        }

        public void InitializeState<TBuildingState>() where TBuildingState : BuildingState
        {
            _stateMachine.Enter<TBuildingState>();
        }

        public void Buy()
        {
            _currenciesModel.Consume(CostType, Cost);
            _stateMachine.Enter<ConstructionState>();
        }
    }
}