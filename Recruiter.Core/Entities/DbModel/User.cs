using System;
using System.ComponentModel.DataAnnotations;

namespace Recruiter.Core.Entities.DbModel
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
