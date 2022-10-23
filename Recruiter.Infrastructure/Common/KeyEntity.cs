using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiter.Infrastructure.Common
{
    public interface IGuidKeyEntity
    {
        public Guid Id { get; set; }
    }

    public interface IIntKeyEntity
    {
        public int Id { get; set; }
    }
}
