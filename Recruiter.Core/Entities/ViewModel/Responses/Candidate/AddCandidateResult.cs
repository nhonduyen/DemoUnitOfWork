using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Responses.Candidate
{
    public class AddCandidateResult : BaseResult
    {
        public AddCandidatesResultData data { get; set; }
        public AddCandidateResult()
        {
            data = new AddCandidatesResultData();
        }
        public class AddCandidatesResultData
        {
            public CandidateResultVM Candidate { get; set; }
        }
    }
}
