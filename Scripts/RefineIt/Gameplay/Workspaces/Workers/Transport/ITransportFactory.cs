using System.Threading.Tasks;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Workers.Path;

namespace Gameplay.Workspaces.Workers.Transport
{
    public interface ITransportFactory
    {
        Task<TransportModel> Create(TransportType transportType, IStorageModel importStorage, IStorageModel exportStorage,
            BezierCurve importCurve, BezierCurve exportCurve, string guid);
    }
}