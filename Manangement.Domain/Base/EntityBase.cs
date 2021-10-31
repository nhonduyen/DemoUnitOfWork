using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Domain.Base
{
    public abstract class EntityBase : IEntityBase
    {
        [Key]
        public virtual Guid Id { get; set; }
    }
}
