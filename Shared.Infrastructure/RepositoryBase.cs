using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Shared.Domain.Interfaces;
using Shared.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Shared.Infrastructure
{
    public class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntityBase
        where TContext : DbContext
    {
        private readonly DbFactoryBase<TContext> _dbFactory;
        private DbSet<TEntity> _dbSet;

        protected DbSet<TEntity> Dbset => _dbSet ??= _dbFactory.Context.Set<TEntity>();

        protected RepositoryBase(DbFactoryBase<TContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public void Add(TEntity entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(TEntity)))
            {
                ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
            }
            Dbset.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(TEntity)))
            {
                ((IAuditEntity)entity).IsDeleted = true;
                Dbset.Update(entity);
            }
            else
                Dbset.Remove(entity);
        }

        public IQueryable<TEntity> List(Expression<Func<TEntity, bool>> expression)
        {
            return Dbset.Where(expression);
        }

        public void Update(TEntity entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(TEntity)))
            {
                ((IAuditEntity)entity).UpdatedDate = DateTime.UtcNow;
            }
            Dbset.Update(entity);
        }
    }
}
