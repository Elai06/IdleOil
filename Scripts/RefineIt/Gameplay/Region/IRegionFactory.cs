using Gameplay.Region.Data;

namespace Gameplay.Region
{
    public interface IRegionFactory
    {
        RegionModel CreateSelectedRegionModel();
    }
}