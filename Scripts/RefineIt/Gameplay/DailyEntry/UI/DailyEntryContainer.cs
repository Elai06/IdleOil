using System;
using System.Collections.Generic;
using Gameplay.DailyEntry.UI;
using Gameplay.Region.Storage;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.DailyEntry
{
    public class DailyEntryContainer: MonoBehaviour
    {
        [SerializeField] public List<SubViewContainerData> SubViewList = new();

        public void UpdateSubData(DailyEntrySubData data, DailyEntryType day)
        {
            SubViewList.Find(x => x.Day == day).SubView.Initialize(data);
        }

        public DailyEntrySubView GetSubView(DailyEntryType day)
        {
            return SubViewList.Find(x => x.Day == day).SubView;
        }
    }

    [Serializable]
    public struct SubViewContainerData
    {
        public DailyEntryType Day;
        public DailyEntrySubView SubView;
    }
}