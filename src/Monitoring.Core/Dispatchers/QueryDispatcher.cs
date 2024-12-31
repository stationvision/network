using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries;

namespace Monitoring.Core.Dispatchers;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _provider;
    private static readonly Type QueryHandlerType = typeof(IQueryHandler<>);

    public QueryDispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }

    public QueryResult Dispatch(Query query)
    {
        var queryHandler = CreateQueryHandler(query);
        return queryHandler.Handle(query, this);
    }

    private IQueryHandler CreateQueryHandler(Query query)
    {
        var queryType = query.GetType();
        var handlerType = QueryHandlerType.MakeGenericType(queryType);
        var queryHandler = (IQueryHandler)_provider.GetService(handlerType);
        return queryHandler;
    }
}