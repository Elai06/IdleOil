using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Currencies.Coefficients;
using Gameplay.DailyEntry;
using Gameplay.MoneyBox;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.PromoCode;
using Gameplay.Quests;
using Gameplay.Region.Data;
using Gameplay.Workers;
using Gameplay.Settings;
using Gameplay.Shop;
using Gameplay.Store;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Gameplay.Workspaces.Workers.Wagon;
using Infrastructure.Purchasing;
using Infrastructure.StaticData.ProcessingWorkspace;
using Infrastructure.Windows;

namespace Infrastructure.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        ProcessingWorkspaceStaticData GetProcessingWorkspaceData(ProcessingType id);
        MiningWorkspaceStaticData MiningWorkspaceStaticData { get; }
        CurrencyCoefficientsStaticData CurrencyCoefficientsStaticData { get; }
        StorageOilCrudeStaticData StorageOilCrudeStaticData { get; }
        TransportPathsStaticData TransportPathsStaticData { get; }
        PipelinesTilemapStaticData PipelinesTilemapData { get; }
        OrderConfig OrderConfigData { get; }
        OrderTutorialConfig OrderTutorialConfig { get; }
        QuestStaticData DailyQuestData { get; }
        QuestStaticData WeeklyQuestData { get; }
        TransportOrderPathsStaticData TransportOrderPathsStaticData { get; }
        WorkersConfig WorkersConfig { get; }
        AudioData AudioData { get; }
        Dictionary<TransportType, TransportStaticData> TransportsData { get; }
        PromoCodesConfig PromoCodesConfig { get; }
        ShopConfigData ShopConfigData { get; }
        DailyEntryStaticData DailyEntryData { get; }
        ClientSpriteStorage ClientSpriteStorage { get; }
        MoneyBoxConfig MoneyBoxConfig { get; }
        CurrencyStaticData GetCurrencyData(CurrencyType currencyType);
        WindowStaticData GetWindowData(WindowType windowType);
        ComplexWorkspaceStaticData GetComplexWorkspaceStaticData(ComplexType complexType);
        ResourceStaticData GetResourceStaticData(ResourceType resourceType);
        RegionConfigData GetRegionConfigData(RegionType regionType);
        TransportStaticData GetTransportData(TransportType transportType);
        PurchaseStaticData GetPurchase(string title);
        IEnumerable<PurchaseStaticData> GetInAppPurchases();
        void LoadRegionConfigs();
        void LoadRegionConfig(RegionType regionType);
        CurrencyCoefficientsData GetCurrencyCoefficientsData(ResourceType resourceType);
        TransportOrderStaticData GetWagonData(TransportOrderType transportOrderType);
        void LoadWagonData(RegionType regionType);
        void LoadWagonsPathsData(RegionType regionType);
        PipelineStaticData GetPipelineStaticData(TransportType transportType);
    }
}