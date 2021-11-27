using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Recruiter.Core.Entities.ViewModel.Requests.Candidate;
using Recruiter.Core.Entities.ViewModel.Responses.Candidate;
using RecruiterDBModel = Recruiter.Core.Entities.DbModel;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Core.Helper;

namespace Recruiter.API.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IUnitOfWorkGeneric<RecruiterContext> _recruiterUow;
        private readonly RecruiterContext _recruiterContext;
        public RecruiterService(IUnitOfWorkGeneric<RecruiterContext> recruiterUow, RecruiterContext recruiterContext)
        {
            _recruiterUow = recruiterUow;
            _recruiterContext = recruiterContext;
        }
        public async Task<int> AddCandidate()
        {
            var recruiter = new RecruiterDBModel.Recruiter();
            recruiter.Name = $"rec_{Guid.NewGuid():N}";

            var candidate1 = new Candidate();
            candidate1.Name = $"can_{Guid.NewGuid():N}";
            candidate1.RecruiterId = recruiter.Id;
            var candidate2 = new Candidate();
            candidate2.Name = $"can_{Guid.NewGuid():N}";
            candidate2.RecruiterId = recruiter.Id;


            _recruiterContext.Repository<Candidate>().AddRange(candidate1, candidate2);
            recruiter.Candidates.Add(candidate1);
            recruiter.Candidates.Add(candidate2);
            _recruiterContext.Repository<RecruiterDBModel.Recruiter>().Add(recruiter);

            var result = await _recruiterUow.SaveChangesAsync();
            return result;
        }

        public async Task<List<CandidateResultVM>> GetCandidate()
        {
            var candidates = await _recruiterUow.Repository<Recruiter.Core.Entities.DbModel.Recruiter>()
                .AsNoTracking()
                .Include(x => x.Candidates)
                .Select(x => new CandidateResultVM
                {
                    RecruiterId = x.Id,
                    RecruiterName = x.Name,
                    Candidates = x.Candidates.Select(a => new CandidateVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        RecruiterId = a.RecruiterId
                    }).ToList()
                })
                .ToListAsync();
            return candidates;
        }

        public async Task<GetCandidatesResult> GetCandidatePagingAsync(GetCandidatesRequest request)
        {
            var result = new GetCandidatesResult();

            var candidates = await _recruiterUow.Repository<Recruiter.Core.Entities.DbModel.Recruiter>()
                .AsNoTracking()
                .Include(x => x.Candidates)
                .Select(x => new CandidateResultVM
                {
                    RecruiterId = x.Id,
                    RecruiterName = x.Name,
                    Candidates = x.Candidates.Select(a => new CandidateVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        RecruiterId = a.RecruiterId
                    }).ToList()
                })
                .SortBy(x => x.RecruiterId, request.payload.sorting.direction)
                .ThenBy(x => x.RecruiterName, request.payload.sorting.direction)
                .ToListPagedAsync(request.payload, result.data);
            result.data.Candidates = candidates;
            return result;
        }
    }
}
