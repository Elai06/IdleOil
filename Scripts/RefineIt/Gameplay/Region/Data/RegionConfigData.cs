using Gameplay.Currencies.Coefficients;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Personnel;
using Gameplay.PromoCode;
using Gameplay.Shop;
using Gameplay.Store;
using Gameplay.Workers;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Gameplay.Workspaces.Workers.Wagon;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine;

namespace Gameplay.Region.Data
{
    [CreateAssetMenu(fileName = "RegionConfig", menuName = "Data/RegionConfigData")]
    public class RegionConfigData : ScriptableObject
    {
        public RegionType RegionType;
        public ComplexWorkspaceStaticData[] ComplexWorkspaceStaticDatas;
        public CurrencyCoefficientsStaticData CurrencyCoefficientsData;
        public MiningWorkspaceStaticData MiningWorkspaceStaticData;
        public ProcessingWorkspaceStaticData[] ProcessingWorkspaceStaticData;
        public StorageOilCrudeStaticData StorageOilCrudeStaticData;
        public TransportStaticData[] Transports;
        public TransportPathsStaticData TransportPaths;
        public PipelinesTilemapStaticData PipelinesTilemapStaticData;
        public PipelineStaticData[] PipelineStaticData;
        public OrderConfig OrderConfigData;
        public OrderTutorialConfig OrderTutorialConfig;
        public TransportOrderPathsStaticData transportOrderPaths;
        public TransportOrderStaticData[] Wagons;
        public PromoCodesConfig PromoCodesConfig;
        public WorkersConfig WorkersConfig;
        public ClientSpriteStorage ClientSpriteStorage;
        public ShopConfigData shopConfigData;
    }
}