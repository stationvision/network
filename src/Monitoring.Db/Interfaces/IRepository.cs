using System.Linq.Expressions;

namespace Monitoring.Db.Interfaces
{
    public interface IRepository<T>
    {
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        IQueryable<T> Query(Expression<Func<T, bool>> predicate);
        T Get(object id);
        void Attach(T entity);
        T Add(T entity);
        int SaveChanges();
        void AddRange(IEnumerable<T> entities);
        List<T> GetAll();
    }
}
