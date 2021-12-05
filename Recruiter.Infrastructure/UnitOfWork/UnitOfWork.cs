using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recruiter.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecruiterContext _recruiterContext;

        public UnitOfWork(RecruiterContext recruiterContext)
        {
            _recruiterContext = recruiterContext;
        }

        public RecruiterContext GetContext()
        {
            return _recruiterContext;
        }

        public DbSet<T> Repository<T>() where T : class
        {
            return _recruiterContext.Repository<T>();
        }

        public int SaveChanges()
        {
            return _recruiterContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _recruiterContext.SaveChangesAsync();
        }
    }
}
