using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class ProcessingWindow : PayloadWindow<ProcessingWorkspaceModel>
    {
        [SerializeField] private ProcessingViewInitializer _processingViewInitializer;
        [SerializeField] private BuildProcessingButton _buildProcessingButton;
        [SerializeField] private UpdateProcessingButton _updateProcessingButton;

        public override void OnOpen(ProcessingWorkspaceModel payload)
        {
            _processingViewInitializer.Initialize(payload);
            _buildProcessingButton.Initialize(payload);
            _updateProcessingButton.Initialize(payload);
        }
    }
}