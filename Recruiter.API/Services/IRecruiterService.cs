using System.Collections.Generic;
using System.Threading.Tasks;
using Recruiter.API.ViewModel;
using Recruiter.API.ViewModel.Requests.Candidate;
using Recruiter.API.ViewModel.Responses.Candidate;

namespace Recruiter.API.Services
{
    public interface IRecruiterService
    {
        Task<int> AddCandidate();
        Task<List<VMCandidate>> GetCandidate();
        Task<GetCandidatesResult> GetCandidatePagingAsync(GetCandidatesRequest request);
    }
}
