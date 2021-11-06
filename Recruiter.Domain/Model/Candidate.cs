using System;
using System.ComponentModel.DataAnnotations;

namespace Recruiter.Domain.Model
{
    public class Candidate : BaseModel
    {
        public Candidate()
        {
            Id = Guid.NewGuid();
            CreatedTime = LastSavedTime = DateTime.UtcNow;
            IsActive = true;
        }
        [Key]
        public Guid Id { get; set; }
        public Guid RecruiterId { get; set; }
        public string Name { get; set; }
        public Guid LastSavedUser { get; set; }
        public DateTime LastSavedTime { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }
    }
}
