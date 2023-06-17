using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class ComplexWindow : PayloadWindow<ComplexWorkspaceModel>
    {
        [SerializeField] private ComplexViewInitializer _complexViewInitializer;
        [SerializeField] private BuildComplexButton _buildComplexButton;
        [SerializeField] private UpdateComplexButton _updateComplexButton;
        
        public override void OnOpen(ComplexWorkspaceModel complex)
        {
            _complexViewInitializer.Initialize(complex);
            _updateComplexButton.Initialize(complex);
            _buildComplexButton.Initialize(complex);
        }
    }
}