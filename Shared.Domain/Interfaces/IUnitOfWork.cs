using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Interfaces
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task CommitAsync();

        TContext Context { get; }
    }
}
