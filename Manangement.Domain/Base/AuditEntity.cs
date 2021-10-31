using System;

namespace Management.Domain.Base
{
    public abstract class AuditEntity : DeleteEntity, IAuditEntity
    {
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
