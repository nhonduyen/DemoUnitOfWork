using System;
using Microsoft.EntityFrameworkCore;
using Recruiter.Core.Entities.DbModel.Bases;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Recruiter.Infrastructure
{
    public interface IUnitOfWorkGeneric<T> where T : DbContext, IContext, IDisposable
    {
        T GetContext(bool isMultiThread = false);
        void Dispose();
        DbSet<TEntity> Repository<TEntity>() where TEntity : BaseModel;
        Task<int> SaveChangesAsync();
        int SaveChanges();
        Task<List<TEntity>> GetLargeWhereInSqlTempTableAsync<TEntity>(List<Guid> listWhereInIds, Func<bool, List<TEntity>> func, int maxWhereIn = 300); 
    }
}
