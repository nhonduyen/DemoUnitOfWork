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
        Task<GetCandidatesResult> GetCandidate();
        Task<GetCandidatesResult> GetCandidatePagingAsync(GetCandidatesRequest request);
        Task<GetCandidatesResult> GetCandidatesByIdsAsync(List<Guid> ids);
        Task<List<Recruiter.Core.Entities.DbModel.Recruiter>> GetCandidatesByIdsAsync2(List<Guid> ids);
    }
}
