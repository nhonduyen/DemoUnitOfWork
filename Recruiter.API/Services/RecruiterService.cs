using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Domain.Model;
using Recruiter.Infrastructure;
using Recruiter.API.ViewModel;
using Microsoft.EntityFrameworkCore;

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
            var recruiter = new Domain.Model.Recruiter();
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
            _recruiterContext.Repository<Domain.Model.Recruiter>().Add(recruiter);

            var result = await _recruiterUow.SaveChangesAsync();
            return result;
        }

        public async Task<List<VMCandidate>> GetCandidate()
        {
            var candidates = await _recruiterUow.Repository<Recruiter.Domain.Model.Recruiter>()
                .AsNoTracking()
                .Include(x => x.Candidates)
                .Select(x => new VMCandidate
                {
                    RecruiterId = x.Id,
                    RecruiterName = x.Name,
                    Candidates = x.Candidates.Select(a => new Candidate
                    {
                        Id = a.Id,
                        Name = a.Name,
                        RecruiterId = a.RecruiterId
                    }).ToList()
                })
                .ToListAsync();
            return candidates;
        }

    }
}
