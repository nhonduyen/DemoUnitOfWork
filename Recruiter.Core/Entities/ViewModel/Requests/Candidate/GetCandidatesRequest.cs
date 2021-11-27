using System.ComponentModel.DataAnnotations;

namespace Recruiter.Core.Entities.ViewModel.Requests.Candidate
{
    public class GetCandidatesRequest : BaseRequest
    {
        [Required]
        public GetCandidatesRequestPayload payload { get; set; }
    }

    public class GetCandidatesRequestPayload : BaseRequestPayload
    {
        public int numberFromRequest { get; set; }
    }
}
