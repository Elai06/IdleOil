using System.Collections;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Workspaces.Buildings.States
{
    public class MiningWorkingState : WorkingState
    {
        private readonly ICoroutineService _coroutineService;
        private readonly MiningWorkspaceStaticData _data;
        private readonly IRegionStorage _regionStorage;
        private Coroutine _miningCoroutine;
        private readonly IWindowService _windowService;
        private readonly MiningWorkSpaceModel _miningModel;
        private readonly IQuestModel _quest;


        public MiningWorkingState(ICoroutineService coroutineService, MiningWorkspaceStaticData data, IRegionStorage regionStorage,
            IWindowService windowService, MiningWorkSpaceModel miningModel, IQuestModel quest)
        {
            _coroutineService = coroutineService;
            _data = data;
            _regionStorage = regionStorage;
            _windowService = windowService;
            _miningModel = miningModel;
            _quest = quest;
        }

        public override void Enter()
        {
            _miningCoroutine = _coroutineService.StartCoroutine(Mining());
        }

        public override void Exit()
        {
            if(_miningCoroutine != null)
                _coroutineService.StopCoroutine(_miningCoroutine);
        }

        public override async void OnClick()
        {
            await _windowService.Open(WindowType.MiningWindow, _miningModel);
        }

        private IEnumerator Mining()
        {
            float currentTimer = 0;
            while(true)
            {
                currentTimer += Time.deltaTime;
                if(currentTimer >= _data.MiningDuration)
                {
                    var bonusResource = _miningModel.GetResourceBonus(ResourceType.Oil);
                    
                    _regionStorage.AddResources(ResourceType.Oil, (int)(_data.MinedResource * bonusResource));
                    _quest.TaskDailyProgress(QuestsGuid.collectOil,1);
                    _quest.TaskWeeklyProgress(QuestsGuid.collectOilWeek,1);
                    currentTimer -= _data.MiningDuration;
                }

                yield return null;
            }

            yield return null;
        }
    }
}