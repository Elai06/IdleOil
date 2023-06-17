using System.Collections;
using System.Linq;
using Gameplay.Services.Timer;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.PersistenceProgress;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Workspaces.Buildings.States
{
    public class ProcessingWorkingState : WorkingState
    {
        private readonly IWindowService _windowService;
        private readonly ProcessingWorkspaceModel _model;
        private readonly TimerService _timerService;

        public ProcessingWorkingState(IWindowService windowService, ProcessingWorkspaceModel model, TimerService timerService)
        {
            _windowService = windowService;
            _model = model;
            _timerService = timerService;
        }

        public override void Enter()
        {
            var timerModel = _timerService.CreateTimer(_model.ProcessingType.ToString(),
                _model.CurrentLevelData.ProcessingTime);
            timerModel.Tick += OnTick;
            timerModel.Stopped += ProcessResource;
            
            TakeInputResources();
        }

        private void OnTick(int time)
        {
            _model.ProcessingTime(time);
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.ProcessingWindow, _model);
        }

        private void ProcessResource(TimeModel model)
        {
            TakeInputResources();

            AddOutputResources();

            StateMachine.Enter<IdleState>();

            model.Stopped -= ProcessResource;
            model.Tick -= OnTick;
        }
        
        private void TakeInputResources()
        {
            foreach (ResourceConversion inputResource in _model.CurrentLevelData.ResourceConversionData.InputResources)
            {
                _model.InputResourceStorage.TakeResources(inputResource.ResourceType, inputResource.Value);
            }
        }

        private void AddOutputResources()
        {
            foreach (ResourceConversion outputResource in
                     _model.CurrentLevelData.ResourceConversionData.OutputResources)
            {
                float bonusProcessing = _model.GetResourceBonus(outputResource.ResourceType);

                _model.OutputResourcesStorage.AddResources(outputResource.ResourceType,
                    (int)(outputResource.Value * bonusProcessing));
            }
        }
    }
}