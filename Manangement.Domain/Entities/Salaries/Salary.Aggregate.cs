using Management.Domain.Users;

namespace Management.Domain.Salaries
{
    public partial class Salary
    {
        const decimal DAY_PRICE = 100;
        public Salary(User user, decimal coefficientSalary, decimal workDays) : base()
        {
            User = user;
            CoefficientsSalary = coefficientSalary;
            WorkingDays = workDays;
            TotalSalary = workDays * DAY_PRICE * coefficientSalary;
        }

        public bool ValidOnAdd()
        {
            return User != null;
        }
    }
}
