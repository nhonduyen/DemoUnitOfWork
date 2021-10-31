using System;
using Management.Domain.Salaries;
using Management.Domain.Users;

namespace Management.Infrastructure.Repositories
{
    public class SalaryRepository : AppRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(AppDbFactory dbFactory) : base(dbFactory)
        {

        }

        public Salary AddUserSalary(User user, decimal coefficientSalary, decimal workdays)
        {
            var salary = new Salary(user, coefficientSalary, workdays);
            if (salary.ValidOnAdd())
            {
                this.Add(salary);
                return salary;
            }
            throw new Exception("Salary invalid");
        }
    }
}
