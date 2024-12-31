using Monitoring.Core.Dispatchers.Interfaces;

namespace Monitoring.Core.Queries;

public interface IQueryHandler<TQuery> : IQueryHandler
{

}

public interface IQueryHandler
{
    QueryResult Handle(IQuery query, IQueryDispatcher dispatcher);
}