using System;
using System.Collections.Generic;
using Recruiter.Domain.Model;

namespace Recruiter.API.ViewModel
{
    public class VMCandidate
    {
        public string RecruiterName { get; set; }
        public Guid RecruiterId { get; set; }
        public IEnumerable<Candidate> Candidates { get; set; }
    }
}
