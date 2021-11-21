using System;

namespace Recruiter.Domain.Model
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
