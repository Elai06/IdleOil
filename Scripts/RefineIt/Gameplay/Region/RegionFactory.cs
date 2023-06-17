using Gameplay.Region.Data;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Region
{
    public class RegionFactory : IRegionFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _playerProgress;

        public RegionFactory(IStaticDataService staticDataService, IProgressService playerProgress)
        {
            _staticDataService = staticDataService;
            _playerProgress = playerProgress;
        }

        public RegionModel CreateSelectedRegionModel()
        {
            return new RegionModel(_playerProgress.RegionProgress,
                _staticDataService.GetRegionConfigData(_playerProgress.RegionProgress.RegionType));
        }
    }
}