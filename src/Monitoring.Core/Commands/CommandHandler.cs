namespace Monitoring.Core.Commands;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
{
    public abstract void Handle(TCommand command);

    void ICommandHandler.Handle(ICommand command)
    {
        Handle((TCommand)command);
    }
}