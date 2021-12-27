using System;
using System.ComponentModel.DataAnnotations;
using Management.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Management.Domain.Departments;
using Management.Domain.Salaries;
using System.Collections.Generic;

namespace Management.Domain.Users
{
    [Table("Users")]
    public partial class User : DeleteEntity
    {
        public User()
        {
        }

        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Guid DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public string Password { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
