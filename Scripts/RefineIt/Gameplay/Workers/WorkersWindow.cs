using Gameplay.Workers;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Personnel
{
    public class WorkersWindow : Window
    {
       [SerializeField] private WorkersViewInitializer workersViewInitializer;
       private WorkersService _workersService;

       [Inject]
       private void Construct(WorkersService workersService)
       {
           _workersService = workersService;
       }

       private void OnEnable()
       {
           workersViewInitializer.Initialize(_workersService);
       }
    }
}