using System;
using Microsoft.EntityFrameworkCore;
using Management.Infrastructure;

namespace Management.Infrastructure
{
    public class AppDbFactory : IDisposable
    {
        private bool _disposed;
        private readonly Func<AppDbContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ??= _instanceFunc.Invoke();

        public AppDbFactory(Func<AppDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
