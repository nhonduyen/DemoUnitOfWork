using System;
using Management.Domain.Base;

namespace Management.Domain.Base
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
    }
}
