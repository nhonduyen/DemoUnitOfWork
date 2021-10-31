using Management.Domain.Interfaces;
using Management.Domain.Departments;

namespace Management.Domain.Users
{
    public interface IUserRepository : IAppRepository<User>
    {
        User NewUser(string username, string email, Department department);
    }
}
