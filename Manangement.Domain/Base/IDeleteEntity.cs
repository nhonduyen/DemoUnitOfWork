namespace Management.Domain.Base
{
    public interface IDeleteEntity : IEntityBase
    {
        bool IsDeleted { get; set; }
    }
}
