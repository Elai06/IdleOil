using System;
using System.Collections.Generic;
using System.Linq;
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
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Gameplay.Workspaces.Workers.Wagon;
using Infrastructure.Purchasing;
using Infrastructure.StaticData.ProcessingWorkspace;
using Infrastructure.Windows;
using UnityEngine;

namespace Infrastructure.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly Dictionary<ProcessingType, ProcessingWorkspaceStaticData> _processingWorkspacesData = new();
        private readonly Dictionary<WindowType, WindowStaticData> _windowsData = new();
        private readonly Dictionary<CurrencyType, CurrencyStaticData> _currenciesData = new();
        private readonly Dictionary<ComplexType, ComplexWorkspaceStaticData> _complexWorkspaceStaticData = new();
        private readonly Dictionary<ResourceType, ResourceStaticData> _resourcesStaticData = new();
        private readonly Dictionary<RegionType, RegionConfigData> _regionConfigDatas = new();
        private readonly Dictionary<TransportType, TransportStaticData> _transportsData = new();
        private readonly Dictionary<TransportType, PipelineStaticData> _pipeline = new();
        private readonly Dictionary<TransportOrderType, TransportOrderStaticData> _wagonsData = new();
        private readonly Dictionary<string, PurchaseStaticData> _purchasesData = new();
        
        public PromoCodesConfig PromoCodesConfig { get; private set; }
        public MiningWorkspaceStaticData MiningWorkspaceStaticData { get; private set; }
        public CurrencyCoefficientsStaticData CurrencyCoefficientsStaticData { get; private set; }
        public StorageOilCrudeStaticData StorageOilCrudeStaticData { get; private set; }
        public TransportPathsStaticData TransportPathsStaticData { get; private set; }
        public TransportOrderPathsStaticData TransportOrderPathsStaticData { get; private set; }
        public WorkersConfig WorkersConfig { get; private set; }
        public PipelinesTilemapStaticData PipelinesTilemapData { get; private set; }
        public OrderConfig OrderConfigData { get; private set; }
        public OrderTutorialConfig OrderTutorialConfig { get; private set; }
        public AudioData AudioData { get; private set; }
        public QuestStaticData DailyQuestData { get; private set; }
        public QuestStaticData WeeklyQuestData { get; private set; }
        public ShopConfigData ShopConfigData { get; private set; }
        public DailyEntryStaticData DailyEntryData { get; private set; }
        public ClientSpriteStorage ClientSpriteStorage { get; private set; }
        public MoneyBoxConfig MoneyBoxConfig { get; private set; }

        public Dictionary<TransportType, TransportStaticData> TransportsData => _transportsData;

        public void Load()
        {
            LoadConfigs();
        }

        public CurrencyCoefficientsData GetCurrencyCoefficientsData(ResourceType resourceType)
        {
            foreach (var currencyCoefficients in CurrencyCoefficientsStaticData
                         .CurrencyCoefficientsData)
            {
                if (resourceType == currencyCoefficients.ResourceType)
                {
                    return currencyCoefficients;
                }
            }

            throw new InvalidOperationException(
                $"Currency data with type: {resourceType} doesn't stored in staticData Service");
        }

        public CurrencyStaticData GetCurrencyData(CurrencyType currencyType)
        {
            if (_currenciesData.TryGetValue(currencyType, out var currencyStaticData))
                return currencyStaticData;

            throw new InvalidOperationException(
                $"Currency data with type: {currencyType} doesn't stored in staticData Service");
        }

        public WindowStaticData GetWindowData(WindowType windowType)
        {
            if (_windowsData.TryGetValue(windowType, out var windowStaticData))
                return windowStaticData;

            throw new InvalidOperationException(
                $"WindowStaticData with type: {windowType} doesn't stored in StaticDataService");
        }

        public ProcessingWorkspaceStaticData GetProcessingWorkspaceData(ProcessingType id)
        {
            if (_processingWorkspacesData.TryGetValue(id, out var data))
                return data;

            throw new InvalidOperationException(
                $"ProcessingWorkspaceStaticData with id: {id} doesn't stored in StaticDataService");
        }

        public ComplexWorkspaceStaticData GetComplexWorkspaceStaticData(ComplexType complexType)
        {
            if (_complexWorkspaceStaticData.TryGetValue(complexType, out var data))
                return data;

            throw new InvalidOperationException(
                $"ComplexWorkspaceStaticData with id: {complexType} doesn't stored in StaticDataService");
        }

        public ResourceStaticData GetResourceStaticData(ResourceType resourceType)
        {
            if (_resourcesStaticData.TryGetValue(resourceType, out var resourceStaticData))
                return resourceStaticData;

            throw new InvalidOperationException(
                $"ResourceData with id: {resourceType} doesn't stored in StaticDataService");
        }

        public RegionConfigData GetRegionConfigData(RegionType regionType)
        {
            if (_regionConfigDatas.TryGetValue(regionType, out var regionConfigData))
            {
                return regionConfigData;
            }

            throw new InvalidOperationException(
                $"ConfigData with id: {regionType} doesn't stored in StaticDataService");
        }

        public TransportStaticData GetTransportData(TransportType transportType)
        {
            if (_transportsData.TryGetValue(transportType, out var transportStaticData))
                return transportStaticData;

            throw new InvalidOperationException(
                $"Config for transport {transportType} doesn't stored in StaticDataService");
        }

        public PipelineStaticData GetPipelineStaticData(TransportType transportType)
        {
            if (_pipeline.TryGetValue(transportType, out var transportStaticData))
                return transportStaticData;

            throw new InvalidOperationException(
                $"Config for pipeline {transportType} doesn't stored in StaticDataService");
        }

        public TransportOrderStaticData GetWagonData(TransportOrderType transportOrderType)
        {
            if (_wagonsData.TryGetValue(transportOrderType, out var wagonStaticData))
                return wagonStaticData;

            throw new InvalidOperationException(
                $"Config for wagon {transportOrderType} doesn't stored in StaticDataService");
        }
        
        public PurchaseStaticData GetPurchase(string title)
        {
            if (_purchasesData.TryGetValue(title, out var data))
                return data;

            throw new InvalidOperationException($"Purchase {title} is not defined");
        }
        
        public IEnumerable<PurchaseStaticData> GetInAppPurchases()
        {
            return _purchasesData.Values.Where(data => data.IsInApp);
        }

        public void LoadRegionConfig(RegionType regionType)
        {
            LoadProcessingWorkspace(regionType);
            LoadMiningWorkspace(regionType);
            LoadStorageOilCrude(regionType);
            LoadComplexWorkspace(regionType);
            LoadCurrencyCoefficients(regionType);
            LoadTransportData(regionType);
            LoadTransportPathsData(regionType);
            LoadPipelinesData(regionType);
            LoadOrderConfigData(regionType);
            LoadWagonData(regionType);
            LoadWagonsPathsData(regionType);
            LoadPromoCodesConfig(regionType);
            LoadWorkersConfig(regionType);
            LoadPipelineData(regionType);
            LoadClientSpriteStorage(regionType);
            LoadStoreConfig(regionType);
        }

        private void LoadPromoCodesConfig(RegionType regionType)
        {
            PromoCodesConfig = _regionConfigDatas[regionType].PromoCodesConfig;
        }

        private void LoadStoreConfig(RegionType regionType)
        {
            ShopConfigData = _regionConfigDatas[regionType].shopConfigData;
        }

        private void LoadWorkersConfig(RegionType regionType)
        {
            WorkersConfig = _regionConfigDatas[regionType].WorkersConfig;
        }

        private void LoadOrderConfigData(RegionType regionType)
        {
            OrderConfigData = _regionConfigDatas[regionType].OrderConfigData;
            OrderTutorialConfig = _regionConfigDatas[regionType].OrderTutorialConfig;
        }

        private void LoadTransportPathsData(RegionType regionType)
        {
            TransportPathsStaticData = _regionConfigDatas[regionType].TransportPaths;
        }

        public void LoadWagonsPathsData(RegionType regionType)
        {
            TransportOrderPathsStaticData = _regionConfigDatas[regionType].transportOrderPaths;
        }

        private void LoadPipelinesData(RegionType regionType)
        {
            PipelinesTilemapData = _regionConfigDatas[regionType].PipelinesTilemapStaticData;
        }

        private void LoadClientSpriteStorage(RegionType regionType)
        {
            ClientSpriteStorage = _regionConfigDatas[regionType].ClientSpriteStorage;
        }

        private void LoadConfigs()
        {
            LoadWindows();
            LoadCurrencies();
            LoadResources();
            LoadQuestData();
            LoadAudioData();
            LoadMoneyBoxConfig();
            LoadPurchases();
        }

        private void LoadTransportData(RegionType regionType)
        {
            foreach (var transport in _regionConfigDatas[regionType].Transports)
                _transportsData[transport.Type] = transport;
        }

        private void LoadPipelineData(RegionType regionType)
        {
            foreach (var transport in _regionConfigDatas[regionType].PipelineStaticData)
                _pipeline[transport.TransportType] = transport;
        }

        public void LoadWagonData(RegionType regionType)
        {
            foreach (var wagonData in _regionConfigDatas[regionType].Wagons)
                _wagonsData[wagonData.Type] = wagonData;
        }

        private void LoadProcessingWorkspace(RegionType regionType)
        {
            foreach (var processingWorkspaceStatic in _regionConfigDatas[regionType]
                         .ProcessingWorkspaceStaticData)
                _processingWorkspacesData[processingWorkspaceStatic.Type] = processingWorkspaceStatic;
        }

        private void LoadCurrencies()
        {
            var currencyStaticDatas =
                Resources.LoadAll<CurrencyStaticData>("Configs/Currencies");
            foreach (var currencyStaticData in currencyStaticDatas)
                _currenciesData[currencyStaticData.CurrencyType] = currencyStaticData;
        }

        private void LoadWindows()
        {
            var windowStaticDatas = Resources.LoadAll<WindowStaticData>("Configs/Windows");
            foreach (var windowStaticData in windowStaticDatas)
                _windowsData.Add(windowStaticData.Type, windowStaticData);
        }

        private void LoadMoneyBoxConfig()
        {
            MoneyBoxConfig = Resources.Load<MoneyBoxConfig>("Configs/MoneyBox/MoneyBoxConfig");
        }

        private void LoadPurchases()
        {
            var purchases = Resources.LoadAll<PurchaseStaticData>("Configs/Purchases");
            foreach (var purchase in purchases)
                _purchasesData.Add(purchase.Title, purchase);
        }
        
        private void LoadMiningWorkspace(RegionType regionType)
        {
            MiningWorkspaceStaticData = _regionConfigDatas[regionType].MiningWorkspaceStaticData;
        }

        private void LoadStorageOilCrude(RegionType regionType)
        {
            StorageOilCrudeStaticData =
                _regionConfigDatas[regionType].StorageOilCrudeStaticData;
        }

        private void LoadCurrencyCoefficients(RegionType regionType)
        {
            CurrencyCoefficientsStaticData =
                _regionConfigDatas[regionType].CurrencyCoefficientsData;
        }

        private void LoadComplexWorkspace(RegionType regionType)
        {
            var complexWorkspaceStaticDatas =
                _regionConfigDatas[regionType].ComplexWorkspaceStaticDatas;

            foreach (var complexWorkspaceStaticData in complexWorkspaceStaticDatas)
                _complexWorkspaceStaticData.Add(complexWorkspaceStaticData.ComplexType, complexWorkspaceStaticData);
        }

        private void LoadResources()
        {
            var resourceStaticDatas =
                Resources.LoadAll<ResourceStaticData>("Configs/GameResources");
            foreach (var resourceStaticData in resourceStaticDatas)
                _resourcesStaticData[resourceStaticData.ResourceType] = resourceStaticData;
        }

        public void LoadRegionConfigs()
        {
            var regionConfigDatas =
                Resources.LoadAll<RegionConfigData>("Configs/Region");
            foreach (var regionConfigData in regionConfigDatas)
                _regionConfigDatas.Add(regionConfigData.RegionType, regionConfigData);
        }

        private void LoadQuestData()
        {
            DailyQuestData = Resources.Load<QuestStaticData>("Configs/Quests/DailyQuest");
            WeeklyQuestData = Resources.Load<QuestStaticData>("Configs/Quests/WeeklyQuest");
            DailyEntryData = Resources.Load<DailyEntryStaticData>("Configs/Quests/DailyEntryData");
        }

        private void LoadAudioData()
        {
            AudioData = Resources.Load<AudioData>("Configs/AudioData");
        }
    }
}