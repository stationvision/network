//using Monitoring.Db.Interfaces;

//namespace NetworkCommunications.Services
//{
//    public class ScopedServiceResolver : IScopedServiceResolver
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public ScopedServiceResolver(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public T Resolve<T>()
//        {
//            using var scope = _serviceProvider.CreateScope();
//            return scope.ServiceProvider.GetRequiredService<T>();
//        }
//    }

//}
