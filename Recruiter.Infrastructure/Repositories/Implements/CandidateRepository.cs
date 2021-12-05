using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Infrastructure.Repositories.Interfaces;
using Recruiter.Infrastructure.UnitOfWork;

namespace Recruiter.Infrastructure.Repositories.Implements
{
    public class CandidateRepository : Repository, ICandidateRepository
    {
        public CandidateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public Candidate AddCandidate(Candidate candidate)
        {
            _unitOfWork.Repository<Candidate>().Add(candidate);
            return candidate;
        }

        public async Task<Candidate> GetCandidateById(Guid id)
        {
            var candidate = await _unitOfWork.Repository<Candidate>().FindAsync(id);
            return candidate;
        }

        public Candidate RemoveCandidate(Candidate candidate)
        {
            _unitOfWork.Repository<Candidate>().Remove(candidate);
            return candidate;
        }

        public Candidate UpdateCandidate(Candidate candidate)
        {
            _unitOfWork.Repository<Candidate>().Update(candidate);
            return candidate;
        }
    }
}
