using Gameplay.Quests;
using Infrastructure.SaveLoads;
using UnityEngine;

namespace Infrastructure.StateMachine.Sates
{
    public class ExitState : IState
    {
        private readonly ISaveLoadService _saveLoadService;

        public ExitState(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            
        }

        public void Enter()
        {
            _saveLoadService.Save();
        }

        public void Exit()
        {

        }
    }
}