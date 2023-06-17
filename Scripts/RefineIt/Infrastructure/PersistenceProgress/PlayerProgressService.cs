namespace Infrastructure.PersistenceProgress
{
    public class PlayerProgressService : IProgressService
    {
        public PlayerProgress PlayerProgress { get; private set; }
        public RegionProgress RegionProgress => PlayerProgress.GetRegionProgress();
        
        public void InitializeProgress(PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;
        }
    }
}