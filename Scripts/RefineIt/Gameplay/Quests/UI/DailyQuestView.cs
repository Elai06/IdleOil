using System;
using Gameplay.Region.Storage;
using Infrastructure.Windows.MVVM.SubView;
using MVVMLibrary.Base.ViewModel;
using UnityEngine;

namespace Gameplay.Quests.UI
{
    public class DailyQuestView : MonoBehaviour
    {
        [field:SerializeField] public DailyQuestContainer DailyQuestContainer{ get; private set; }
    }
}