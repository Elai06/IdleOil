using Gameplay.Region;
using Gameplay.Settings;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class BuildComplexButton : ButtonBaseSFX
    {
        private ComplexWorkspaceModel _complexWorkspaceModel;

        public override void OnClick()
        {
            base.OnClick();
            if(_complexWorkspaceModel.CanBuy)
                _complexWorkspaceModel.Buy();
        }

        public void Initialize(ComplexWorkspaceModel complex)
        {
            _complexWorkspaceModel = complex;
        }
    }
}