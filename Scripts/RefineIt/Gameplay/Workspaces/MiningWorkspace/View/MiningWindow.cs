using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Workspaces.MiningWorkspace.View
{
    public class MiningWindow : PayloadWindow<MiningWorkSpaceModel>
    {
        [SerializeField] private MiningViewInitializer _miningViewInitializer;
        [SerializeField] private ButtonMiningBuild _button;
        
        public override void OnOpen(MiningWorkSpaceModel mining)
        {
            _miningViewInitializer.Initialize(mining);
            _button.Initialize(mining);
        }
    }
}