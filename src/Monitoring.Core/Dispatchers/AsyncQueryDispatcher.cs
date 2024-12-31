using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries;

namespace Monitoring.Core.Dispatchers
{
    public class AsyncQueryDispatcher : IAsyncQueryDispatcher
    {
        private readonly IServiceProvider _provider;
        private static readonly Type AsyncQueryHandlerType = typeof(IAsyncQueryHandler<>);

        public AsyncQueryDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<QueryResult> DispatchAsync(Query query)
        {
            var asyncQueryHandler = CreateAsyncQueryHandler(query);
            return await asyncQueryHandler.HandleAsync(query, this);
        }

        private IAsyncQueryHandler CreateAsyncQueryHandler(Query query)
        {
            var queryType = query.GetType();
            var handlerType = AsyncQueryHandlerType.MakeGenericType(queryType);
            var asyncQueryHandler = (IAsyncQueryHandler)_provider.GetService(handlerType);
            return asyncQueryHandler;
        }

    }
}
