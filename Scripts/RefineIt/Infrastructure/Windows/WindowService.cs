using System.Collections.Generic;
using System.Threading.Tasks;
using MVVMLibrary.Enums;
using UnityEngine;

namespace Infrastructure.Windows
{
    public class WindowService : IWindowService
    {
        private readonly Dictionary<WindowType, Window> _cashedWindows = new Dictionary<WindowType, Window>();
        private readonly Dictionary<Layer, Window> _windowsOnLayer = new Dictionary<Layer, Window>();
        private readonly IWindowFactory _windowFactory;

        public WindowService(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public Dictionary<WindowType, Window> CashedWindows => _cashedWindows;

        public async Task<Window> Open(WindowType windowType)
        {
            if (_cashedWindows.TryGetValue(windowType, out var window))
            {
                window.gameObject.SetActive(true);
                CloseMenuWindowWhenOpenGameplayLayer(window);
                CloseWindowOnSameLayer(window);
                return window;
            }

            var windowInstance = await _windowFactory.Create(windowType);
            windowInstance.SetType(windowType);

            if (_cashedWindows.ContainsKey(windowType))
            {
                Debug.LogWarning($"This screen already exists");
                return null;
            }

            _cashedWindows.Add(windowType, windowInstance);
            CloseWindowOnSameLayer(windowInstance);
            return windowInstance;
        }


        private void CloseWindowOnSameLayer(Window window)
        {
            if (_windowsOnLayer.TryGetValue(window.Layer, out var openedWindow))
                if (window != openedWindow)
                    Close(openedWindow.WindowType);

            _windowsOnLayer[window.Layer] = window;
        }

        public async Task<Window> Open<TPaylaod>(WindowType windowType, TPaylaod paylaod)
        {
            var window = await Open(windowType);
            if (window is PayloadWindow<TPaylaod> payloadWindow)
                payloadWindow.OnOpen(paylaod);

            CloseMenuWindowWhenOpenGameplayLayer(window);
            return window;
        }

        public void Close(WindowType windowType)
        {
            if (_cashedWindows.TryGetValue(windowType, out var window))
                window.gameObject.SetActive(false);
                    
            OpenMenuWhenClosedGameplayLayer(window);
        }

        private void CloseMenuWindowWhenOpenGameplayLayer(Window window)
        {
            if (window.Layer == Layer.BuildingWindow)
            {
                Close(WindowType.Menu);
            }
        }

        private async void OpenMenuWhenClosedGameplayLayer(Window window)
        {
            if (window.Layer == Layer.BuildingWindow)
            {
                await Open(WindowType.Menu);
            }
        }
    }
}