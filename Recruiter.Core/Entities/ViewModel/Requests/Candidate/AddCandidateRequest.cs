using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Requests.Candidate
{
    public class AddCandidateRequest : BaseRequest
    {
        [Required]
        public AddCandidatesRequestPayload payload { get; set; }
    }

    public class AddCandidatesRequestPayload
    {
        public CandidateModel Candidate { get; set; }
    }

    public class CandidateModel
    {
       public string Name { get; set; }
       public Guid RecruiterId { get; set; }
       public Guid Id { get; set; }
    }
}
