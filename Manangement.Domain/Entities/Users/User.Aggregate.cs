using Management.Domain.Departments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Domain.Users
{
    public partial class User
    {
        public User(string username, string email, Department department) : base()
        {
            Department = department;
            UserName = username;
            Email = email;
        }

        public bool ValidOnAdd()
        {
            return !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrEmpty(Email)
                && new EmailAddressAttribute().IsValid(Email)
                && Department != null;
        }
    }
}
