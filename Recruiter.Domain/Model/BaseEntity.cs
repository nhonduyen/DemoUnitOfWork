using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recruiter.Domain.Model
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime LastSavedTime { get; set; } = DateTime.UtcNow;
        public Guid CreatedUser { get; set; }
        public Guid LastSavedUser { get; set; }
        public Boolean IsActive { get; set; } = true;
        public void UpdateLastSavedEntity(Guid userId, DateTime dateTime)
        {
            this.LastSavedUser = userId;
            this.LastSavedTime = dateTime;
        }
        [NotMapped]
        public IList<string> ChangedFields { get; set; } = new List<string>();
        [NotMapped]
        public static readonly string[] DefaultUpdatedFields = new string[]
        {
            nameof(LastSavedUser),
            nameof(LastSavedTime)
        };
    }
}
