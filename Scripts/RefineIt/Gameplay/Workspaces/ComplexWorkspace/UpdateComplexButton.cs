using Gameplay.Region;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class UpdateComplexButton : ButtonBaseSFX
    {
        private ComplexWorkspaceModel _complexWorkspaceModel;

        public override void OnClick()
        {
            base.OnClick();
            if(_complexWorkspaceModel.CanUpdateLevel())
                _complexWorkspaceModel.UpdateLevel();
        }

        public void Initialize(ComplexWorkspaceModel complex)
        {
            _complexWorkspaceModel = complex;
        }
    }
}