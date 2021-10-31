using Management.Domain.Interfaces;
using Management.Domain.Users;

namespace Management.Domain.Salaries
{
    public interface ISalaryRepository : IAppRepository<Salary>
    {
        Salary AddUserSalary(User user, decimal coefficientSalary, decimal workdays);
    }
}
