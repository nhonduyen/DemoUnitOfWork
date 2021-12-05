using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recruiter.Core.Entities.DbModel.Bases
{
    public class BaseEntity : Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime LastSavedTime { get; set; } = DateTime.UtcNow;
        public Guid CreatedUser { get; set; }
        public Guid LastSavedUser { get; set; }
        public Boolean IsActive { get; set; } = true;

        public void UpdateCreatedEntity(Guid userId, DateTime dateTime)
        {
            this.CreatedUser = userId;
            this.CreatedTime = dateTime;
        }

        public void UpdateLastSavedEntity(Guid userId, DateTime dateTime)
        {
            this.LastSavedUser = userId;
            this.LastSavedTime = dateTime;
        }

        [NotMapped]
        public static readonly string[] DefaultUpdatedFields = new string[]
        {
            nameof(LastSavedUser),
            nameof(LastSavedTime)
        };
    }

    public abstract class Entity
    {
        [NotMapped]
        public IList<string> ChangedFields { get; set; } = new List<string>();
    }
}
