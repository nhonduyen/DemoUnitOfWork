using System.Threading.Tasks;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Core.Entities.ViewModel;
using Recruiter.Core.Entities.ViewModel.Requests;
using System;

namespace Recruiter.API.Services
{
    public interface IUserService
    {
        Task<User> Login(string username, string password);
        Task<TokenInfoVM> ProcessLogin(string username, string password);
        Task<User> GetUserByUsername(string username);
        Task<int> UpdateRefreshToken(string username, string refreshToken);
        Task<int> UpdateRefreshToken(User user, string token);
    }
}
