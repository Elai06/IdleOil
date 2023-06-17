using DG.Tweening;
using Gameplay.Settings.UI;
using Gameplay.Workspaces.MiningWorkspace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Windows.MVVM.SubView
{
    public class StorageResourceView : SubView<IconDescriptionData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _popUp;
        [SerializeField] private TextMeshProUGUI _popUpText;

        private Tween _popUpTween;

        public override void Initialize(IconDescriptionData data)
        {
            _image.sprite = data.Sprite;
            _text.text = data.Description;
            _popUpText.text = $"{GamesResourcesName.GetResourceName(data.Id)}";
        }

        public void OnOpenPopUp()
        {
            _popUp.SetActive(!_popUp.activeSelf);
            _popUpTween?.Kill();
            _popUpTween = DOVirtual.DelayedCall(2, () => _popUp.SetActive(false));
        }

        public void ClosePopUp()
        {
            _popUp.SetActive(false);
            _popUpTween?.Kill();
        }
    }
}