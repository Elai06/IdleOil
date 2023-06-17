using Gameplay.Region;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class BuildProcessingButton : ButtonBaseSFX
    {
        private ProcessingWorkspaceModel _processingWorkspaceModel;

        public override void OnClick()
        {
            base.OnClick();
            if (_processingWorkspaceModel.CanBuy)
            {
                _processingWorkspaceModel.Buy();
            }
        }

        public void Initialize(ProcessingWorkspaceModel complex)
        {
            _processingWorkspaceModel = complex;
        }
    }
}