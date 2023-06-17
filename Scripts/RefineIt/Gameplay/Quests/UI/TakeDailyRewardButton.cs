using System;
using Gameplay.Quests;
using Gameplay.Region;

namespace Gameplay.Orders
{
    public class TakeDailyRewardButton : ButtonBaseSFX
    {
        public event Action<int,QuestsGuid> ClickButton;
        private int _reward;
        private QuestsGuid _guid;

        public void Initialize(int reward, QuestsGuid guid)
        {
            _reward = reward;
            _guid = guid;
        }

        public override void OnClick()
        {
            base.OnClick();
            ClickButton?.Invoke(_reward, _guid);
            transform.parent.gameObject.SetActive(false);
        }
    }
}