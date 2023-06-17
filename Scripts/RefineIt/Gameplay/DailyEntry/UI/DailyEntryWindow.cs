using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.DailyEntry
{
    public class DailyEntryWindow : Window
    {
        [SerializeField] private DailyEntryInitializer _dailyEntryInitializer;
        private IDailyEntryModel _model;
        
        [Inject]
        private void Construct(IDailyEntryModel model)
        {
            _model = model;
        }
        
        private void OnEnable()
        {
            _dailyEntryInitializer.Initialize(_model);
        }
    }
}