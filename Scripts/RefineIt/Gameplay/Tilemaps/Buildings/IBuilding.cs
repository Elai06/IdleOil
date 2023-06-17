using System;
using Gameplay.Workspaces.Buildings.States;
using UnityEngine;

namespace Gameplay.Tilemaps.Buildings
{
    public interface IBuilding
    {
        event Action OnStateChange;
        event Action<IBuilding> Builded;
        Vector3Int Guid { get; }
        string Id { get; }
        bool IsBuilded { get; }
        void InitializeState<TBuildingState>() where TBuildingState : BuildingState;
        BuildingState CurrentState { get; }
        bool IsConstructing { get; }
        void Buy();
        void Build();
        void OnClick();
    }
}