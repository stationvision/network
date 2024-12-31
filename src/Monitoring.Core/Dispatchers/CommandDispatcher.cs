using Monitoring.Core.Commands;
using Monitoring.Core.Dispatchers.Interfaces;

namespace Monitoring.Core.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _provider;
    private static readonly Type CommandHandlerTyper = typeof(ICommandHandler<>);

    public CommandDispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }

    private ICommandHandler? CreateCommandHandler(Command command)
    {
        var queryType = command.GetType();
        var handlerType = CommandHandlerTyper.MakeGenericType(queryType);
        var commandHandler = (ICommandHandler)_provider.GetService(handlerType);
        return commandHandler;
    }
    public void Dispatch(Command cmd)
    {
        var handler = CreateCommandHandler(cmd);
        handler.Handle(cmd);

    }
}