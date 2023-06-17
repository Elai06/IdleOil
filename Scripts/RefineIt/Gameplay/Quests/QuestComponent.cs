using System;
using Gameplay.Currencies;
using Gameplay.Quests.UI;
using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine;

namespace Gameplay.Quests
{
    [Serializable]
    public class QuestComponent
    {
        public Sprite Sprite;    
        public string description;
        public int Reward;
        public int Count;
        public bool WIP;
        public QuestsGuid QuestsGuid;
        public ResourceType resource;
        public QuestType quest;
        [NonSerialized] public QuestsProgress QuestProgress;
    }
}