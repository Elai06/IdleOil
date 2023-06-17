using Gameplay.Region.Data;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Region
{
    public class RegionModel
    {
        public readonly RegionProgress RegionProgress;
        private readonly RegionConfigData _regionConfigData;

        public RegionModel(RegionProgress regionProgress, RegionConfigData regionConfigData)
        {
            RegionProgress = regionProgress;
            _regionConfigData = regionConfigData;
        }
        
        public RegionType RegionType => _regionConfigData.RegionType;
    }
}