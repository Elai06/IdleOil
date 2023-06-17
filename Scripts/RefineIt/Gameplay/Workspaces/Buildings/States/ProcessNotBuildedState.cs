using Gameplay.CameraProject;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.Windows;

namespace Gameplay.Workspaces.Buildings.States
{
    public class ProcessNotBuildedState : NotBuildedState
    {
        private readonly IWindowService _windowService;
        private readonly ProcessingWorkspaceModel _model;
        private readonly CameraZoomController _cameraZoomController;

        public ProcessNotBuildedState(IWindowService windowService, ProcessingWorkspaceModel model, CameraZoomController cameraZoomController)
        {
            _windowService = windowService;
            _model = model;
            _cameraZoomController = cameraZoomController;
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.ProcessingWindow, _model);
        }

        public override void Enter()
        {
            // TODO Open Process Window
        }

        public override void Exit()
        {
            _windowService.Close(WindowType.ProcessingWindow);
            _cameraZoomController.ZoomOut();
        }
    }
}