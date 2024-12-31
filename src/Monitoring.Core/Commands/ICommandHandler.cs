namespace Monitoring.Core.Commands;

public interface ICommandHandler
{
    void Handle(ICommand command);
}

public interface ICommandHandler<TCommand> : ICommandHandler
{
    void Handle(TCommand command);
}

public interface ICommandHandler<TCommand, TCommandResult> : ICommandHandler
{
    CommandResult<TCommandResult> Handle(TCommand cmd);
}