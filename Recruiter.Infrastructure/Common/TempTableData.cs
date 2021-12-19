using System;
using System.ComponentModel.DataAnnotations;

namespace Recruiter.Infrastructure.Common
{
    public class TempTableData
    {
        [Key]
        public Guid Id { get; set; }
    }
}
