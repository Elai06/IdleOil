using Gameplay.Region.Storage;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public interface IExportStorage
    {
        IStorageModel ExportStorage { get; }
    }
}