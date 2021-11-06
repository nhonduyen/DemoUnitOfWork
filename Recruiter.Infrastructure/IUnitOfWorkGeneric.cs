using System;
using Microsoft.EntityFrameworkCore;
using Recruiter.Domain.Model;
using System.Threading.Tasks;

namespace Recruiter.Infrastructure
{
    public interface IUnitOfWorkGeneric<T> where T : DbContext, IContext, IDisposable
    {
        T GetContext(bool isMultiThread = false);
        void Dispose();
        DbSet<TEntity> Repository<TEntity>() where TEntity : BaseModel;
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
