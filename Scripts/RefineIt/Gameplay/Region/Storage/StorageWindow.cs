using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Region.Storage
{
    public class StorageWindow : Window
    {
        [SerializeField] private StorageViewInitializer _storageViewInitializer;
        private IRegionStorage _regionStorage;

        [Inject]
        private void Construct(IRegionStorage storageModel)
        {
            _regionStorage = storageModel;
        }

        private void Awake()
        {
            _storageViewInitializer.Initialize(_regionStorage);
        }
    }
}