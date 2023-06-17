using System;
using Gameplay.Quests;
using Gameplay.Region;

namespace Gameplay.Orders
{
    public class RefreshDailyButton : ButtonBaseSFX
    {
        public event Action<QuestsGuid, bool> ClickButton;
        private QuestsGuid _guid;
        private bool _isDaily;

        public void Initialize(QuestsGuid guid, bool isDaily)
        {
            _guid = guid;
            _isDaily = isDaily;
        }
        
        public override void OnClick()
        {
            base.OnClick();
            ClickButton?.Invoke(_guid, _isDaily);
        }
    }
}