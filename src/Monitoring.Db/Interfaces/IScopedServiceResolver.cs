namespace Monitoring.Db.Interfaces
{
    public interface IScopedServiceResolver
    {
        T Resolve<T>();
    }

}
