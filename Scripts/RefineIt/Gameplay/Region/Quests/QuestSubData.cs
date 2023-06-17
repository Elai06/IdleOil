using Gameplay.Quests;
using UnityEngine;

namespace Infrastructure.Windows.MVVM.SubView
{
    public class QuestSubData
    {
        public Sprite Sprite;
        public string Description;
        public string Reward;
        public int Count;
        public int Progress;
        public QuestsGuid Guid;
        public bool IsDaily;
        
        public QuestSubData(Sprite sprite, string description, QuestsGuid guid, string reward, int count, int progress
        , bool isDaily)
        {
            Sprite = sprite;
            Description = description;
            Guid = guid;
            Reward = reward;
            Count = count;
            Progress = progress;
            IsDaily = isDaily;
        }
    }
}