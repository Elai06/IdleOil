using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Windows.MVVM.SubView
{
    public class IconDescriptionView : SubView<IconDescriptionData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;

        public void ViewSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void ViewValue(string description)
        {
            _text.text = description;
        }

        public override void Initialize(IconDescriptionData data)
        {
            _image.sprite = data.Sprite;
            _text.text = data.Description;
        }
    }
}