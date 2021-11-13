using System;
using System.Collections.Generic;

namespace Recruiter.API.ViewModel.Responses.Candidate
{
    public class GetCandidatesResult : BaseResult
    {
        public GetCandidatesResultData data { get; set; }
        public GetCandidatesResult()
        {
            data = new GetCandidatesResultData();
        }
    }

    public class GetCandidatesResultData : BaseResultData
    {
        public IEnumerable<CandidateResultVM> Candidates { get; set; }
    }

    public class CandidateResultVM
    {
        public string RecruiterName { get; set; }
        public Guid RecruiterId { get; set; }
        public IEnumerable<CandidateVM> Candidates { get; set; }
    }
    public class CandidateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RecruiterId { get; set; }
    }
}
