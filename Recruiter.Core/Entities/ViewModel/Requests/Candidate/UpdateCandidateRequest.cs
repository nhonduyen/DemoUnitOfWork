using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Requests.Candidate
{
    public class UpdateCandidateRequest : BaseRequest
    {
        [Required]
        public AddCandidatesRequestPayload payload { get; set; }
    }

    public class UpdateCandidatesRequestPayload
    {
        public CandidateModel Candidate { get; set; }
    }
}
