namespace Monitoring.Core.Queries;

public class Query : IQuery
{
    public string Name => GetType().Name;
}