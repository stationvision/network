using Microsoft.EntityFrameworkCore;
using Monitoring.Core.Interfaces;
using Monitoring.Db.Interfaces;
using System.Linq.Expressions;

namespace Monitoring.Db.Repositories
{
    //public class AsyncIdentityRepository<T> : IAsyncRepository<T> where T : class, IEntity
    public class AsyncIdentityRepository<T> : IAsyncRepository<T> where T : class, IEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly IdentityMonitoringDbContext _identityMonitoringDbContext;

        public AsyncIdentityRepository(IdentityMonitoringDbContext identityMonitoringDbContext)
        {
            _dbSet = identityMonitoringDbContext.Set<T>();
            _identityMonitoringDbContext = identityMonitoringDbContext;
        }

        public async Task<T> GetAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            var added = await _dbSet.AddAsync(entity);
            await _identityMonitoringDbContext.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _identityMonitoringDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _identityMonitoringDbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}
