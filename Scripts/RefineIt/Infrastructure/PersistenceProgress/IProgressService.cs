namespace Infrastructure.PersistenceProgress
{
    public interface IProgressService
    {
        PlayerProgress PlayerProgress { get; }
        RegionProgress RegionProgress { get; }
        void InitializeProgress(PlayerProgress playerProgress);
    }
}