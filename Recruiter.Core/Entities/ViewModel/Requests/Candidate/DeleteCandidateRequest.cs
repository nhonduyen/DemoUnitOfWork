using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Requests.Candidate
{
    public class DeleteCandidateRequest : BaseRequest
    {
        [Required]
        public DeleteCandidatesRequestPayload payload { get; set; }
    }

    public class DeleteCandidatesRequestPayload
    {
        public CandidateModel Candidate { get; set; }
    }
}
