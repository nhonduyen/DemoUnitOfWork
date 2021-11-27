using System.ComponentModel.DataAnnotations;

namespace Recruiter.Core.Entities.ViewModel.Requests.User
{
    public class LoginRequest
    {
        [Required]
        public LoginRequestPayload payload { get; set; }
    }

    public class LoginRequestPayload
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
