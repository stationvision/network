namespace Monitoring.Core.Commands;

public class Command : ICommand
{
    public string Name => GetType().Name;
}