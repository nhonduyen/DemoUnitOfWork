using System;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace Shared.Infrastructure
{
    public class DbFactoryBase<TContext> : IDbFactory<TContext> where TContext : DbContext 
    {
        private bool _disposed;
        private readonly Func<TContext> _instanceFunc;
        private TContext _dbContext;
        public TContext Context => _dbContext ??= _instanceFunc.Invoke();

        public DbFactoryBase(Func<TContext> dbContextFactory)
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
