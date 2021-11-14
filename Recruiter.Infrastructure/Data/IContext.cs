using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Recruiter.Infrastructure
{
    public interface IContext : IDisposable
    {
        T CreateContext<T>(IOptions<ConnectionStringsSetting> connectionOptions) where T : class;
        DbSet<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
