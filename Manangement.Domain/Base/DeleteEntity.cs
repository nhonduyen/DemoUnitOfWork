namespace Management.Domain.Base
{
    public abstract class DeleteEntity : EntityBase, IDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}
