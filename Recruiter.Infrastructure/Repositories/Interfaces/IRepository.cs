using System;
using System.Collections.Generic;
using Recruiter.Core.Entities.DbModel.Bases;

namespace Recruiter.Infrastructure.Repositories.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : Entity;
        T Add<T>(T entity) where T : Entity;
        IEnumerable<T> AddRange<T>(IEnumerable<T> entity) where T : Entity;
        T Update<T>(T entity) where T : Entity;
        T Remove<T>(T entity) where T : Entity;
    }
}
