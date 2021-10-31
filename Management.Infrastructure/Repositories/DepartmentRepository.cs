using Management.Domain.Departments;
using System;
using Microsoft.AspNetCore.Http;
using Shared.Infrastructure;
using Shared.Domain.Base;

namespace Management.Infrastructure.Repositories
{
    public class DepartmentRepository : AppRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbFactory dbFactory) : base(dbFactory)
        {

        }
        public Department AddDepartment(string deparmentName)
        {
            var department = new Department(deparmentName);
            if (department.ValidOnAdd())
            {
                this.Add(department);
                return department;
            }
            throw new Exception("Invalid deparment");
        }
    }
}
