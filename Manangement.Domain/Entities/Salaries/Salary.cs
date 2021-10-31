using System;
using System.ComponentModel.DataAnnotations.Schema;
using Management.Domain.Base;
using Management.Domain.Users;

namespace Management.Domain.Salaries
{
    [Table("Salaries")]
    public partial class Salary : AuditEntity
    {
        public Salary()
        {

        }
        public Guid UserId { get; set; }
        public decimal CoefficientsSalary { get; set; }
        public decimal WorkingDays { get; set; }
        public decimal TotalSalary { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
