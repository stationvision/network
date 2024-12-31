namespace Monitoring.Core.Commands;

public class CommandResult<TCommandResult> : CommandResult
{
    public TCommandResult Data => (TCommandResult)RowData;

    public CommandResult(TCommandResult result) : base(result)
    {

    }
}

public class CommandResult : ICommandResult
{
    protected object RowData { get; set; }

    public CommandResult(object rowData)
    {
        RowData = rowData;
    }
}
