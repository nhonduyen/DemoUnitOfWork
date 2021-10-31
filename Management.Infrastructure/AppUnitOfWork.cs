using Shared.Infrastructure;
using Management.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Management.Infrastructure
{
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly AppDbFactory _appDbFactory;
        public AppUnitOfWork(AppDbFactory dbFactory)
        {
            _appDbFactory = dbFactory;
        }

        public Task<int> CommitAsync()
        {
            return _appDbFactory.DbContext.SaveChangesAsync();
        }
    }
}
