using System;
using Management.Domain.Departments;
using Management.Domain.Users;

namespace Management.Infrastructure.Repositories
{
    public class UserRepository : AppRepository<User>, IUserRepository
    {
        public UserRepository(AppDbFactory dbFactory) : base(dbFactory)
        {

        }
        public User NewUser(string username, string email, Department department, string password)
        {
            var user = new User(username, email, department, password);
            
            if (user.ValidOnAdd())
            {
                this.Add(user);
                return user;
            }
            throw new Exception("Department invalid");
        }
    }
}
