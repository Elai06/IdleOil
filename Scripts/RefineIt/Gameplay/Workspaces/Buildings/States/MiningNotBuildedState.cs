using Gameplay.CameraProject;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.Windows;

namespace Gameplay.Workspaces.Buildings.States
{
    public class MiningNotBuildedState : NotBuildedState
    {
        private readonly IWindowService _windowService;
        private readonly MiningWorkSpaceModel _miningWorkSpaceModel;
        private readonly CameraZoomController _cameraZoomController;

        public MiningNotBuildedState(IWindowService windowService, MiningWorkSpaceModel miningWorkSpaceModel, CameraZoomController cameraZoomController)
        {
            _windowService = windowService;
            _miningWorkSpaceModel = miningWorkSpaceModel;
            _cameraZoomController = cameraZoomController;
        }

        public override void Enter()
        {
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.MiningWindow, _miningWorkSpaceModel);
            
        }

        public override void Exit()
        {
            _windowService.Close(WindowType.MiningWindow);
            _cameraZoomController.ZoomOut();
        }
    }
}