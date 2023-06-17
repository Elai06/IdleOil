using Gameplay.Region.Storage;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public interface IImportStorage
    {
        IStorageModel ImportStorage { get; }
    }
}