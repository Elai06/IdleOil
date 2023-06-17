using System.Collections.Generic;
using Gameplay.Tilemaps.Data;
using Gameplay.Currencies;
using Gameplay.Workspaces.Buildings;

namespace Gameplay.Workspaces.MiningWorkspace
{
    public class MiningWorkSpaceModel : BuildingBase<MiningWorkspaceProgress, MiningWorkspaceStaticData>
    {
        private Dictionary<ResourceType, float> _bonusResources = new();

        public MiningWorkSpaceModel(MiningWorkspaceProgress progress, MiningWorkspaceStaticData data, 
            CurrenciesModel currenciesModel) : base(progress, data, currenciesModel)
        {
        }

        public override string Id => BuildingType.Mining.ToString();
        public string Description => Data.Description;

        public void AddMiningBonus(ResourceType resourceType, float value)
        {
            value /= 100;
            
            if (_bonusResources.ContainsKey(resourceType))
            {
                _bonusResources[resourceType] = value;
            }
            else
            {
                _bonusResources.Add(resourceType, value);
            }
        }

        public float GetResourceBonus(ResourceType resourceType)
        {
            if (_bonusResources.ContainsKey(resourceType))
            {
                return _bonusResources[resourceType] + 1;
            }

            return 1;
        }
    }
}