using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;

namespace Gameplay.Region.Storage
{
    public class RegionView : MonoBehaviour
    {
        [field: SerializeField]
        public SubViewContainer<StorageResourceView, IconDescriptionData> SubViewContainer { get; private set; }


        public void CloseAllResourcePopUps()
        {
            foreach (var subView in SubViewContainer.SubViews.Values)
            {
                subView.ClosePopUp();
            }
        }
    }
}