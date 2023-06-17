using Gameplay.Region;
using Gameplay.Workspaces.Buildings.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.CameraProject
{
    public class ZoomOut : ButtonBase
    {
        private CameraZoomController _zoomController;
        
        [Inject]
        private void Construct(CameraZoomController zoomController)
        {
            _zoomController = zoomController;
        }

        public override void OnClick()
        {
            _zoomController.ZoomOut();
        }
    }
}