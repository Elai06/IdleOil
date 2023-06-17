using Gameplay.Workers;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.Personnel
{
    public class WorkersView : MonoBehaviour
    {
        [field: SerializeField] public SubViewContainer<WorkerSubView, WorkersViewData> WorkerSubView { get; private set; }
    }
}