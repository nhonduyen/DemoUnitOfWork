using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Recruiter.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        DbSet<T> Repository<T>() where T : class;
        RecruiterContext GetContext();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
