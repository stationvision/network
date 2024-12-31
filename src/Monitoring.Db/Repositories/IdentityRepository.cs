using Microsoft.EntityFrameworkCore;
using Monitoring.Core.Interfaces;
using Monitoring.Db.Interfaces;
using System.Linq.Expressions;

namespace Monitoring.Db.Repositories
{
    public class IdentityRepository<T> : IRepository<T> where T : class, IEntity
    {

        private DbSet<T> dbset;
        private readonly IdentityMonitoringDbContext _identityMonitoringDbContext;

        public IdentityRepository(IdentityMonitoringDbContext identityMonitoringDbContext)
        {
            //identityMonitoringDbContext.Database.EnsureCreated();
            dbset = identityMonitoringDbContext.Set<T>();
            _identityMonitoringDbContext = identityMonitoringDbContext;
        }

        public T Get(object id)
        {
            return dbset.Find(id);
        }

        public void Attach(T entity)
        {
            dbset.Attach(entity);
        }

        public T Add(T entity)
        {
            var added = dbset.Add(entity);

            return added.Entity;
        }

        public int SaveChanges()
        {
            return _identityMonitoringDbContext.SaveChanges();
        }

        public T Update(T entity)
        {
            return dbset.Update(entity).Entity;
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbset.AddRange(entities);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return dbset.Where(predicate);
        }

        public List<T> GetAll()
        {
            return dbset.ToList();
        }
    }
}
