using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Recruiter.Domain.Model;
using Recruiter.Infrastructure.Common;

namespace Recruiter.Infrastructure
{
    public class BaseSqlServerContext : DbContext
    {
        private static readonly string[] defaultFieldsUpdated = new string[]
        {
            nameof(BaseEntity.LastSavedTime),
            nameof(BaseEntity.LastSavedUser)
        };

        public IQueryable<Guid> TempTableData => Set<TempTableData>().FromSqlRaw($"SELECT * FROM #{nameof(TempTableData)}").Select(x => x.Id);

        public BaseSqlServerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TempTableData>()
                  .HasNoKey()
                  .ToView($"#{nameof(TempTableData)}");
        }
    }
}
