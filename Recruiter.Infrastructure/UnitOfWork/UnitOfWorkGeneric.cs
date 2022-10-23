using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recruiter.Core.Entities.DbModel.Bases;
using System.Threading.Tasks;
using System.Collections.Generic;
using Recruiter.Infrastructure.Extensions;
using System.Linq;
using System.Linq.Expressions;

namespace Recruiter.Infrastructure
{
    public class UnitOfWorkGeneric<T> : IUnitOfWorkGeneric<T> where T : DbContext, IContext, IDisposable
    {
        private bool disposed = false;
        private readonly T _dataContext;
        private readonly IOptions<ConnectionStringsSetting> _connectionOptions;

        public UnitOfWorkGeneric(T dataContext, IOptions<ConnectionStringsSetting> connectionOptions)
        {
            _connectionOptions = connectionOptions;
            _dataContext = dataContext;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T GetContext(bool isMultiThread = false)
        {
            if (!isMultiThread) return _dataContext as T;
            return _dataContext as T;
        }

        public DbSet<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            return _dataContext.Repository<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                // implement dispose here
            }
            disposed = true;
        }

        public async Task<List<TEntity>> GetLargeWhereInSqlTempTableAsync<TEntity>(List<Guid> listWhereInIds, Func<bool, List<TEntity>> func, int maxWhereIn = 300)
        {
            return await _dataContext.GetLargeWhereInSqlTempTable(listWhereInIds, func, maxWhereIn);
        }
    }
}
