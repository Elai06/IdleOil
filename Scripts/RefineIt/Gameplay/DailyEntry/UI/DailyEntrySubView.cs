using System;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.DailyEntry.UI
{
    public class DailyEntrySubView : SubView<DailyEntrySubData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _imageTakeReward;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private DailyEntryTakeReward _buttonTake;
        [SerializeField] private GameObject _сheckGO;
        [SerializeField] private TMP_Text _description;

        public override void Initialize(DailyEntrySubData data)
        {
            _description.text = data.Description;
            _image.sprite = data.Sprite;
            _rewardText.text = data.Reward;
            _buttonTake.Initialize(data.Day,data.Currency,int.Parse(data.Reward));
            
            _buttonTake.gameObject.SetActive(data.IsShowRewardTake);
            _сheckGO.SetActive(data.IsTake);
            _imageTakeReward.gameObject.SetActive(data.IsTake);
        }

        public DailyEntryTakeReward GetRewardButton()
        {
            return _buttonTake;
        }

        public void ActiveCheck(bool isActive)
        {
            _сheckGO.SetActive(isActive);
            _imageTakeReward.gameObject.SetActive(isActive);
        }
    }
}