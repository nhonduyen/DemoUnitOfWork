using System.Threading.Tasks;
using Recruiter.Domain.Model;

namespace Recruiter.API.Services
{
    public interface IUserService
    {
        Task<User> Login(string username, string password);
    }
}
