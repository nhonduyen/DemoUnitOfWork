using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Recruiter.Domain.Model;

namespace Recruiter.Infrastructure
{
    public class RecruiterContext : BaseSqlServerContext, IContext, IDisposable,
        IDesignTimeDbContextFactory<RecruiterContext>
    {
        private bool _disposed = false;

        public RecruiterContext(DbContextOptions<RecruiterContext> options) : base(options)
        {

        }

        public virtual RecruiterContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RecruiterContext>();
            return new RecruiterContext(builder.Options);
        }

        public T CreateContext<T>(IOptions<ConnectionStringsSetting> connectionOptions) where T : class
        {
            return CreateDbContext(new string[] { connectionOptions.Value.ManagementConnection }) as T;
        }

        public DbSet<T> Repository<T>() where T : class
        {
            return Set<T>();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            var result = base.SaveChangesAsync();
            return result;
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Entry(entity);
            var entityType = entity.GetType();
            IList<string> changedFields = null;

            if (entityType.GetProperties().Any(x => x.Name == nameof(BaseEntity.ChangedFields)))
            {
                changedFields = (IList<string>)entityType.GetProperty(nameof(BaseEntity.ChangedFields)).GetValue(entity);
            }

            this.HandleEntityUpdateChangedFields(entity, changedFields, BaseEntity.DefaultUpdatedFields);
        }

        #region table
        public DbSet<Recruiter.Domain.Model.Recruiter> Recruiter { get; set; }
        public DbSet<Candidate> Candidate { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Model.Recruiter>()
                .HasIndex(a => a.Id)
                .HasDatabaseName("Recruiter_Id")
                .IsUnique();
            modelBuilder.Entity<Candidate>()
                .HasIndex(a => a.Id)
                .HasDatabaseName("Candidate_Id")
                .IsUnique();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.IsKeyless)
                {
                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(property.GetColumnName().ToLower());
                    }
                }
                else
                {
                    var currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();

                    modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToLower());

                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(property.GetColumnName().ToLower());
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
                // Database.CloseConnection();
            }

            _disposed = true;
        }
    }
}
