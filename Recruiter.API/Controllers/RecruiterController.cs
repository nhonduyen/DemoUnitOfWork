using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.API.Services;

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
        public async Task<ActionResult> AddCandidates()
        {
            await _recruiterService.AddCandidate();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetCandidates()
        {
            var candidates = await _recruiterService.GetCandidate();
            return Ok(candidates);
        }
    }
}
