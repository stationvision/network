using Monitoring.Core.Dispatchers.Interfaces;
using System.Threading.Tasks;

namespace Monitoring.Core.Queries
{
    public abstract class AsyncQueryHandler<TQuery, TResult> : IAsyncQueryHandler<TQuery>
    {
        public abstract Task<QueryResult> HandleAsync(TQuery query, IAsyncQueryDispatcher dispatcher);

        public async Task<QueryResult> HandleAsync(IQuery query, IAsyncQueryDispatcher dispatcher)
        {
            return await HandleAsync((TQuery)query, dispatcher);
        }
    }
}
