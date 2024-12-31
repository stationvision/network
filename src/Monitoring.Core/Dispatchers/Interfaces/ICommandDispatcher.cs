using Monitoring.Core.Commands;

namespace Monitoring.Core.Dispatchers.Interfaces
{
    public interface ICommandDispatcher
    {
        void Dispatch(Command cmd);
    }
}
