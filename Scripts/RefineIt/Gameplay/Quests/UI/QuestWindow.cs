using Gameplay.Region.Storage;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Quests.UI
{
    public class QuestWindow : Window
    {
        [SerializeField] private DailyViewInitializer _dailyViewInitializer;
        [SerializeField] private WeeklyViewInitializer _weeklyViewInitializer;
        private IQuestModel _quest;

        [Inject]
        private void Construct(IQuestModel quest)
        {
            _quest = quest;
        }

        private void OnEnable()
        {
            _dailyViewInitializer.Initialize(_quest);
            _weeklyViewInitializer.Initialize(_quest);
        }
    }
}