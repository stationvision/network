namespace Monitoring.Core.Queries;

public class QueryResult : IQueryResult
{
    public object RowData { get; set; }

    public QueryResult(object rowData)
    {
        RowData = rowData;
    }
}