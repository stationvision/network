using Monitoring.Core.Dispatchers.Interfaces;
using System.Threading.Tasks;

namespace Monitoring.Core.Queries
{
    public interface IAsyncQueryHandler<TQuery> : IAsyncQueryHandler
    {
    }

    public interface IAsyncQueryHandler
    {
        Task<QueryResult> HandleAsync(IQuery query, IAsyncQueryDispatcher dispatcher);
    }
}
