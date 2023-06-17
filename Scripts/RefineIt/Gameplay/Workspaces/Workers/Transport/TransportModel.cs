using System;
using System.Linq;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.States;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;

namespace Gameplay.Workspaces.Workers.Transport
{
    public class TransportModel : IResourceCapacity
    {
        private readonly StateMachine _stateMachine;

        private readonly TransportStaticData _data;
        private readonly TransportProgress _progress;

        public TransportModel(TransportStaticData data, TransportProgress progress, ICoroutineService coroutineService,
            TransportMover mover, BezierCurve shippingPath, BezierCurve returnPath, IStorageModel importStorage, IStorageModel exportStorage)
        {
            Mover = mover;
            _data = data;
            _progress = progress;
            Storage = new StorageModel(progress.StorageProgress, this);
            _stateMachine = new StateMachine();
            _stateMachine.AddState(new TransportIdleState(_progress, importStorage, Storage));
            _stateMachine.AddState(new TransportImportState(this, coroutineService, _progress, Storage, importStorage));
            _stateMachine.AddState(new ShippingState(coroutineService, mover, _progress, this, shippingPath));
            _stateMachine.AddState(new TransportExportState(_progress, this, Storage, exportStorage, coroutineService));
            _stateMachine.AddState(new ReturnState(coroutineService, mover, _progress, this, returnPath));
            InitializeStorage();
            Initialize();
        }


        public IStorageModel Storage { get; private set; }
        public TransportMover Mover { get; private set; }
        
        public event Action<ResourceType, int> CapacityChanged; // TODO invoke on update level
        
        public float ImportTime => CurrentLevel.ImportTime;
        public float ShippingTime => CurrentLevel.ShippingTime;
        public float ExportTime => CurrentLevel.ExportTime;
        public float ReturnTime => CurrentLevel.ReturnTime;
        public TransportLevelData CurrentLevel => _data.Levels[_progress.CurrentLevel];


        // TODO Update Level Logic

        private void InitializeStorage()
        {
            foreach(var currentLevelCapacity in CurrentLevel.Capacities) 
                Storage.AddResources(currentLevelCapacity.ResourceType, 0);
        }
        
        private void Initialize()
        {
            switch (_progress.CurrentState)
            {
                case TransportState.Idle:
                    _stateMachine.Enter<TransportIdleState>();
                    break;
                case TransportState.Import:
                    _stateMachine.Enter<TransportImportState>();
                    break;
                case TransportState.Shipping:
                    _stateMachine.Enter<ShippingState>();
                    break;
                case TransportState.Export:
                    _stateMachine.Enter<TransportExportState>();
                    break;
                case TransportState.Return:
                    _stateMachine.Enter<ReturnState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid Trandport State {_progress.CurrentState}");
            }
        }

        public int GetResourceCapacity(ResourceType resourceType)
        {
            return (from resource in CurrentLevel.Capacities
                where resource.ResourceType == resourceType
                select resource.Capacity).FirstOrDefault();
        }
    }
}