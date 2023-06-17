using UnityEngine;

namespace Infrastructure.Windows.MVVM.SubView
{
    public class IconDescriptionData
    {
        public Sprite Sprite;
        public string Description;
        public string Id;

        public IconDescriptionData(Sprite sprite, string description, string id)
        {
            Sprite = sprite;
            Description = description;
            Id = id;
        }
    }
}