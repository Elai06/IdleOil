using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Windows
{
    public interface IWindowService
    {
        Task<Window> Open(WindowType windowType);
        void Close(WindowType windowType);
        Task<Window> Open<TPaylaod>(WindowType windowType, TPaylaod paylaod);
        Dictionary<WindowType, Window> CashedWindows { get; }
    }
}