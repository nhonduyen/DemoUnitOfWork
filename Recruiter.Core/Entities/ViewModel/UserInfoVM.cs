using System;

namespace Recruiter.Core.Entities.ViewModel
{
    public class UserInfoVM
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
