using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recruiter.Domain.Model
{
    public abstract class BaseModel
    {
        [NotMapped]
        public IList<string> ChangedFields { get; set; } = new List<string>();
    }
}
