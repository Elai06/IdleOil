using Gameplay.Region;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class UpdateProcessingButton : ButtonBaseSFX
    {
        private ProcessingWorkspaceModel _processingViewInitializer;

        public override void OnClick()
        {
            base.OnClick();
            if (_processingViewInitializer.CanUpdateLevel())
            {
                _processingViewInitializer.UpdateLevel();
            }
        }
        
        public void Initialize(ProcessingWorkspaceModel complex)
        {
            _processingViewInitializer = complex;
        }
    }
}