using Monitoring.Core.Queries;
using System.Threading.Tasks;

namespace Monitoring.Core.Dispatchers.Interfaces
{
    public interface IAsyncQueryDispatcher
    {
        Task<QueryResult> DispatchAsync(Query query);
    }
}
