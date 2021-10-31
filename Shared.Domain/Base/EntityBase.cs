using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain.Base
{
    public interface IEntityBase
    {
        [Key]
        Guid Id { get; set; }
    }

    public interface IDeleteEntity
    {
        bool IsDeleted { get; set; }
    }

    public interface IAuditEntity : IDeleteEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        void SetCreator(string creator, DateTime dateTime);
        void SetUpdater(string updater, DateTime dateTime);
    }

    public abstract class EntityBase : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }
    }

    public abstract class DeleteEntity : EntityBase, IDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }

    public abstract class AuditEntity : IDeleteEntity, IAuditEntity
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public void SetCreator(string creator, DateTime dateTime)
        {
            this.CreatedBy = creator;
            this.CreatedDate = dateTime;
        }

        public void SetUpdater(string updater, DateTime dateTime)
        {
            this.UpdatedBy = updater;
            this.UpdatedDate = dateTime;
        }
    }
}
