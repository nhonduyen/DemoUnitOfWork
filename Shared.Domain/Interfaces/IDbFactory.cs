using Microsoft.EntityFrameworkCore;

namespace Shared.Domain
{
    public interface IDbFactory<TContext> where TContext : DbContext
    {
        TContext Context { get; }
    }
}
