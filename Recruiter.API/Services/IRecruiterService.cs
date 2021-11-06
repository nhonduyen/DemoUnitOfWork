using System.Collections.Generic;
using System.Threading.Tasks;
using Recruiter.API.ViewModel;

namespace Recruiter.API.Services
{
    public interface IRecruiterService
    {
        Task<int> AddCandidate();
        Task<List<VMCandidate>> GetCandidate();
    }
}
