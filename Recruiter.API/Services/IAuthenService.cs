using Microsoft.Extensions.Configuration;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Core.Entities.ViewModel;
using Recruiter.Core.Entities.ViewModel.Requests;
using System.Security.Claims;

namespace Recruiter.API.Services
{
    public interface IAuthenService
    {
        public TokenInfoVM RequestToken(User user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public User GetUserFromPrincipal(ClaimsPrincipal claimsPrincipal);
    }
}
