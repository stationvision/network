using Microsoft.Extensions.DependencyInjection;
using Monitoring.Db.Interfaces;

namespace Monitoring.Db.Repositories
{
    public class AsyncRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AsyncRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAsyncRepository<T> CreateAsyncRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<IAsyncRepository<T>>();
        }
    }

}
