using Recruiter.Core.Entities.DbModel;
using System;
using System.Threading.Tasks;

namespace Recruiter.Services.Interface
{
    public interface ICandidateService
    {
        Task<Candidate> GetCanndidateById(Guid id);
        Task<int> InsertCandidate(Candidate candidate);
        Task<int> UpdateCandidate(Candidate candidate);
        Task<int> DeleteCandidate(Candidate candidate);
    }
}
