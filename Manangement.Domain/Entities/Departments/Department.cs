using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Management.Domain.Users;
using Management.Domain.Base;

namespace Management.Domain.Departments
{
    [Table("Departments")]
    public partial class Department : AuditEntity
    {
        public Department()
        {
            Users = new HashSet<User>();
        }
        public string DepartmentName { get; set; }
        public virtual ICollection<User> Users{ get; set; }
    }
}
