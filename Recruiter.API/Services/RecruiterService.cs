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
using Recruiter.Infrastructure.UnitOfWork;
using Recruiter.Infrastructure.Extensions;

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

        public async Task<GetCandidatesResult> GetCandidate()
        {
            GetCandidatesResult result = new GetCandidatesResult();
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
            result.data.Candidates = candidates;
            return result;
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

        public async Task<GetCandidatesResult> GetCandidatesByIdsAsync(List<Guid> ids)
        {
            var result = new GetCandidatesResult();

            List<CandidateResultVM> InlineFunc(bool isUseTempTable)
            {
                var tempQuery = isUseTempTable ? _recruiterContext.TempTableData : ids.AsEnumerable();
                return _recruiterContext.Repository<Recruiter.Core.Entities.DbModel.Recruiter>()
                .AsNoTracking()
                .Include(x => x.Candidates)
                .Where(x => tempQuery.Contains(x.Id))
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
                }).ToList();
            }

            var candidates = await _recruiterUow.GetLargeWhereInSqlTempTableAsync(ids, InlineFunc);
            result.data.Candidates = candidates;
            return result;
        }

        public async Task<List<Recruiter.Core.Entities.DbModel.Recruiter>> GetCandidatesByIdsAsync2(List<Guid> ids)
        {
            var candidates = await _recruiterContext.WhereBulkContainsAsync<Recruiter.Core.Entities.DbModel.Recruiter>
                (
                    listWhereInIds: ids,
                    tempTable: _recruiterUow.GetContext().TempTableData,
                    keyContains: nameof(Recruiter.Core.Entities.DbModel.Recruiter.Id),
                    extraCondition: x => x.IsActive,
                    isTracking: false,
                    selector: x => new Recruiter.Core.Entities.DbModel.Recruiter
                    {
                        Name = x.Name,
                        Candidates = x.Candidates,
                        IsActive = x.IsActive
                    }
                ); 
            return candidates;
        }
    }
}
