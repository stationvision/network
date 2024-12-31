namespace Monitoring.Db.Interfaces
{
    public interface IAsyncRepositoryFactory
    {
        IAsyncRepository<T> CreateAsyncRepository<T>() where T : class;
    }

}
