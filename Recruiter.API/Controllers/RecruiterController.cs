using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Recruiter.API.Services;
using Recruiter.Core.Entities.ViewModel.Requests.Candidate;

namespace Recruiter.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;
        public RecruiterController(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }

        [HttpGet]
        public async Task<IActionResult> AddCandidates()
        {
            var numCandidates = await _recruiterService.AddCandidate();
            return Ok(numCandidates);
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _recruiterService.GetCandidate();
            return Ok(candidates);
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidatesPaging([FromBody] GetCandidatesRequest request)
        {
            var numberFromRequest = request.payload.numberFromRequest;
            var response = await _recruiterService.GetCandidatePagingAsync(request);
            return Ok(response);
        }
    }
}
