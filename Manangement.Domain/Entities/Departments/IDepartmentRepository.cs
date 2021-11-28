using Management.Domain.Base;
using Management.Domain.Interfaces;
using Shared.Domain.Interfaces;
using System;

namespace Management.Domain.Departments
{
    public interface IDepartmentRepository : IAppRepository<Department>
    {
        Department AddDepartment(string deparmentName);
        Department GetDepartmentById(Guid id);
    }
}
