using Gameplay.CameraProject;
using Gameplay.Workspaces.ComplexWorkspace;
using Infrastructure.Windows;

namespace Gameplay.Workspaces.Buildings.States
{
    public class ComplexNotBuildedState : NotBuildedState
    {
        private readonly IWindowService _windowService;
        private readonly ComplexWorkspaceModel _complexWorkspaceModel;
        private readonly CameraZoomController _cameraZoomController;

        public ComplexNotBuildedState(IWindowService windowService, ComplexWorkspaceModel complexWorkspaceModel, CameraZoomController cameraZoomController)
        {
            _complexWorkspaceModel = complexWorkspaceModel;
            _cameraZoomController = cameraZoomController;
            _windowService = windowService;
        }

        public override void Enter()
        {
        }
        
        public override async void OnClick()
        {
            await _windowService.Open(WindowType.ComplexWindow, _complexWorkspaceModel);
        }


        public override void Exit()
        {
            _windowService.Close(WindowType.ComplexWindow);
            _cameraZoomController.ZoomOut();
        }
    }
}