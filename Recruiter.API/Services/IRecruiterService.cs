using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruiter.Core.Entities.ViewModel.Requests.Candidate;
using Recruiter.Core.Entities.ViewModel.Responses.Candidate;

namespace Recruiter.API.Services
{
    public interface IRecruiterService
    {
        Task<int> AddCandidate();
        Task<List<CandidateResultVM>> GetCandidate();
        Task<GetCandidatesResult> GetCandidatePagingAsync(GetCandidatesRequest request);
        Task<GetCandidatesResult> GetCandidatesByIdsAsync(List<Guid> ids);
    }
}
