using System.Linq;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.Windows;

namespace Gameplay.Workspaces.Buildings.States
{
    public class ProcessingIdleState : IdleState
    {
        private readonly ProcessingWorkspaceModel _model;
        private readonly IWindowService _windowService;

        public ProcessingIdleState(ProcessingWorkspaceModel model, IWindowService windowService)
        {
            _model = model;
            _windowService = windowService;
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.ProcessingWindow, _model);
        }

        public override void Enter()
        {
            base.Enter();

            _model.InputResourceStorage.ResourceChanged += OnChangeResourcesStorage;
            _model.OutputResourcesStorage.ResourceChanged += OnChangeResourcesStorage;

            OnChangeResourcesStorage();
        }

        public override void Exit()
        {
            base.Exit();

            _model.InputResourceStorage.ResourceChanged -= OnChangeResourcesStorage;
            _model.OutputResourcesStorage.ResourceChanged -= OnChangeResourcesStorage;
        }

        private void OnChangeResourcesStorage(ResourceType type = ResourceType.Oil, int amount = 0)
        {
            if (CheckCanTakeInputResources() && CheckCanPlaceOutputResources())
            {
                StateMachine.Enter<WorkingState>();
            }
        }

        private bool CheckCanTakeInputResources()
        {
            var inputResourcesData = _model.CurrentLevelData.ResourceConversionData.InputResources;

            return inputResourcesData.All(inputResource =>
                _model.InputResourceStorage.CanTakeResources(inputResource.ResourceType, inputResource.Value));
        }

        private bool CheckCanPlaceOutputResources()
        {
            var outputResourcesData = _model.CurrentLevelData.ResourceConversionData.OutputResources;

            return outputResourcesData.All(outputResource =>
                _model.OutputResourcesStorage.CanPlaceResources(outputResource.ResourceType) >
                outputResource.Value * _model.GetResourceBonus(outputResource.ResourceType));
        }
    }
}