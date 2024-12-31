using Microsoft.Extensions.DependencyInjection;

namespace Monitoring.Core.Extentions
{
    public static class CommandsAndQueriesExtensions
    {
        public static IServiceCollection AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface, Type assemblyLocation)
        {
            Type[] types = assemblyLocation.Assembly.GetTypes();
            var handlers = types
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                Type serviceType = handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);
                services.AddScoped(serviceType, handler);
            }


            return services;
        }
    }
}
