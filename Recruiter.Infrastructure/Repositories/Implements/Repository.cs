using System;
using System.Collections.Generic;
using System.Text;
using Recruiter.Core.Entities.DbModel.Bases;
using Recruiter.Infrastructure.Repositories.Interfaces;
using Recruiter.Infrastructure.UnitOfWork;

namespace Recruiter.Infrastructure.Repositories.Implements
{
    public abstract class Repository
    {
        protected readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public T Add<T>(T entity) where T : Entity
        {
            _unitOfWork.Repository<T>().Add(entity);
            return entity;
        }

        public IEnumerable<T> AddRange<T>(IEnumerable<T> entity) where T : Entity
        {
            _unitOfWork.Repository<T>().AddRange(entity);
            return entity;
        }

        public T GetById<T>(Guid id) where T : Entity
        {
            return _unitOfWork.Repository<T>().Find(id);
        }

        public T Remove<T>(T entity) where T : Entity
        {
            _unitOfWork.Repository<T>().Remove(entity);
            return entity;
        }

        public T Update<T>(T entity) where T : Entity
        {
            _unitOfWork.Repository<T>().Update(entity);
            return entity;
        }
    }
}
