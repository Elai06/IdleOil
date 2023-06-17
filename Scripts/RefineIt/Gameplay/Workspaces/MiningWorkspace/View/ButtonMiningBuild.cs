using Gameplay.Region;

namespace Gameplay.Workspaces.MiningWorkspace.View
{
    public class ButtonMiningBuild : ButtonBaseSFX
    {
        private MiningWorkSpaceModel _miningWorkSpaceModel;

        public override void OnClick()
        {
            base.OnClick();
            if (_miningWorkSpaceModel.CanBuy)
                _miningWorkSpaceModel.Buy();
        }

        public void Initialize(MiningWorkSpaceModel mining)
        {
            _miningWorkSpaceModel = mining;
        }
    }
}