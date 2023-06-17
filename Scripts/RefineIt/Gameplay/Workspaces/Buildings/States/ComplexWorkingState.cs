using Gameplay.Workspaces.ComplexWorkspace;
using Infrastructure.Windows;

namespace Gameplay.Workspaces.Buildings.States
{
    public class ComplexWorkingState : WorkingState
    {
        private readonly IWindowService _windowService;
        private readonly ComplexWorkspaceModel _model;

        public ComplexWorkingState(IWindowService windowService, ComplexWorkspaceModel model)
        {
            _windowService = windowService;
            _model = model;
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.ComplexWindow, _model);
        }
    }
}