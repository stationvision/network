using Monitoring.Core.Dispatchers.Interfaces;

namespace Monitoring.Core.Queries;

public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery>
{
    public abstract QueryResult Handle(TQuery query, IQueryDispatcher dispatcher);

    QueryResult IQueryHandler.Handle(IQuery query, IQueryDispatcher dispatcher)
    {
        return Handle((TQuery)query, dispatcher);
    }
}