using System;
using System.Linq;
using System.Linq.Expressions;
using Shared.Domain.Base;

namespace Shared.Domain.Interfaces
{
    public interface IRepository<T> where T : IEntityBase
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> List(Expression<Func<T, bool>> expression);
    }
}
