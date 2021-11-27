using Recruiter.Core.Entities.DbModel.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recruiter.Core.Entities.DbModel
{
    public class Recruiter : BaseModel
    {
        public Recruiter()
        {
            Id = Guid.NewGuid();
            CreatedTime = LastSavedTime = DateTime.UtcNow;
            IsActive = true;
            Candidates = new List<Candidate>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Candidate> Candidates { get; set; }
        public Guid LastSavedUser { get; set; }
        public DateTime LastSavedTime { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }
    }
}
