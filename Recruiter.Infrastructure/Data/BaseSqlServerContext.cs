using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Recruiter.Domain.Model;

namespace Recruiter.Infrastructure
{
    public class BaseSqlServerContext : DbContext
    {
        private static readonly string[] defaultFieldsUpdated = new string[]
        {
            nameof(BaseEntity.LastSavedTime),
            nameof(BaseEntity.LastSavedUser)
        };

        public BaseSqlServerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
