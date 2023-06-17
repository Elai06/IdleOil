using Gameplay.CameraProject;
using Gameplay.Workspaces.CrudeOilStorage;
using Infrastructure.Windows;

namespace Gameplay.Workspaces.Buildings.States
{
    public class OilStorageNotBuildedState : NotBuildedState
    {
        private readonly IWindowService _windowService;
        private readonly StorageOilCrudeModel _oilStorageModel;
        private readonly CameraZoomController _cameraZoomController;


        public OilStorageNotBuildedState(IWindowService windowService, StorageOilCrudeModel oilStorageModel, CameraZoomController cameraZoomController)
        {
            _windowService = windowService;
            _oilStorageModel = oilStorageModel;
            _cameraZoomController = cameraZoomController;
        }

        public override void Enter()
        {
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.StorageOilWindow, _oilStorageModel);
        }

        public override void Exit()
        {
            _windowService.Close(WindowType.StorageOilWindow);
            _cameraZoomController.ZoomOut();
        }
    }
}