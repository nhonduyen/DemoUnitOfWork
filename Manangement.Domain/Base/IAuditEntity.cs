using System;

namespace Management.Domain.Base
{
    public interface IAuditEntity : IDeleteEntity
    {
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        Guid UpdatedBy { get; set; }
    }
}
