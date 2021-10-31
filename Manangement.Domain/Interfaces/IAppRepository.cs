using System;
using System.Linq;
using System.Linq.Expressions;
using Management.Domain.Interfaces;
using Management.Domain.Base;

namespace Management.Domain.Interfaces
{
    public interface IAppRepository<T> where T : class 
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable List(Expression<Func<T, bool>> expression);
    }
}
