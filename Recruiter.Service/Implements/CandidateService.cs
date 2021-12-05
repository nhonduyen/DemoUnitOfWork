using Recruiter.Core.Entities.DbModel;
using Recruiter.Infrastructure.Repositories.Interfaces;
using Recruiter.Infrastructure.UnitOfWork;
using Recruiter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recruiter.Services.Implement
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CandidateService(ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DeleteCandidate(Candidate candidate)
        {
            _candidateRepository.RemoveCandidate(candidate);
            var result = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<Candidate> GetCanndidateById(Guid id)
        {
            var candidate = await _candidateRepository.GetCandidateById(id);
            return candidate;
        }

        public async Task<int> InsertCandidate(Candidate candidate)
        {
            _candidateRepository.AddCandidate(candidate);
            var result = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<int> UpdateCandidate(Candidate candidate)
        {
            candidate.LastSavedTime = DateTime.UtcNow;
            
            _candidateRepository.UpdateCandidate(candidate);
            var result = await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
