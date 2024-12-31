using Monitoring.Core.Queries;

namespace Monitoring.Core.Dispatchers.Interfaces;

public interface IQueryDispatcher
{
    QueryResult Dispatch(Query query);
}