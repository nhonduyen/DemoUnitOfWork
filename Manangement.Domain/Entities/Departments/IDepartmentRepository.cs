using Management.Domain.Base;
using Management.Domain.Interfaces;
using Shared.Domain.Interfaces;

namespace Management.Domain.Departments
{
    public interface IDepartmentRepository : IAppRepository<Department>
    {
        Department AddDepartment(string deparmentName);
    }
}
