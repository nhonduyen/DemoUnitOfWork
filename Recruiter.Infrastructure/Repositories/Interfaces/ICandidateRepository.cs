using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recruiter.Core.Entities.DbModel;

namespace Recruiter.Infrastructure.Repositories.Interfaces
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetCandidateById(Guid id);
        Candidate AddCandidate(Candidate candidate);
        Candidate UpdateCandidate(Candidate candidate);
        Candidate RemoveCandidate(Candidate candidate);
    }
}
