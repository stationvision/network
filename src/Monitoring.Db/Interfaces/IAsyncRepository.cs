using System.Linq.Expressions;

namespace Monitoring.Db.Interfaces
{
    public interface IAsyncRepository<T>
    {
        Task<T> GetAsync(object id);
        Task<T> AddAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<List<T>> GetAllAsync();
        IQueryable<T> Query(Expression<Func<T, bool>> predicate);
        Task<T> UpdateAsync(T entity);
    }

}
