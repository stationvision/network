using Microsoft.Extensions.DependencyInjection;

namespace Monitoring.Db.Interfaces
{

    public class ScopedServiceFactory : IScopedServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopedServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IServiceScope CreateScope()
        {
            return _serviceProvider.CreateScope();
        }
    }

    public interface IScopedServiceFactory
    {
        IServiceScope CreateScope();
    }

}
