using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Recruiter.Domain.Model
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
