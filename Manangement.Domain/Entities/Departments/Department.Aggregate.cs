using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Domain.Departments
{
    public partial class Department
    {
        public Department(string departmentName) : base()
        {
            this.DepartmentName = departmentName;
        }

        public bool ValidOnAdd()
        {
            return !string.IsNullOrEmpty(this.DepartmentName);
        }
    }
}
