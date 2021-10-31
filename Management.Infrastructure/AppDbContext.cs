using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Management.Domain.Users;
using Management.Domain.Salaries;
using Management.Domain.Departments;

namespace Management.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Salary> Salaries { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salary>().Property(_ => _.CoefficientsSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(_ => _.TotalSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(_ => _.WorkingDays).HasPrecision(18, 2);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Salary>().ToTable("Salary");
            modelBuilder.Entity<Department>().ToTable("Department");
        }
    }
}
