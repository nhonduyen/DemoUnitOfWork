using System.Threading.Tasks;

namespace Management.Domain.Interfaces
{
    public interface IAppUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
