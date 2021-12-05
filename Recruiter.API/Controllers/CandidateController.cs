using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruiter.Core.Controllers;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Core.Entities.ViewModel.Requests.Candidate;
using Recruiter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruiter.API.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class CandidateController : SecureBaseController
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(IHttpContextAccessor contextAccessor, ICandidateService candidateService) : base(contextAccessor)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCandidateRequest request)
        {
            try
            {
                var userId = GetUserIdAuthentication(HttpContext);
                var candidate = new Candidate();
                candidate.CreatedUser = candidate.LastSavedUser = userId.GetValueOrDefault();
                candidate.Name = request.payload.Candidate.Name;
                candidate.RecruiterId = request.payload.Candidate.RecruiterId;
                var result = await _candidateService.InsertCandidate(candidate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = HandleException(ex);
                return result; 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateCandidateRequest request)
        {
            try
            {
                var userId = GetUserIdAuthentication(HttpContext);
                var candidate = new Candidate
                {
                    Id = request.payload.Candidate.Id,
                    Name = request.payload.Candidate.Name,
                    RecruiterId = request.payload.Candidate.RecruiterId,
                    LastSavedUser = userId.GetValueOrDefault()
                };
                var result = await _candidateService.UpdateCandidate(candidate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = HandleException(ex);
                return result;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteCandidateRequest request)
        {
            try
            {
                var candidate = new Candidate
                {
                    Id = request.payload.Candidate.Id
                };
                candidate.Id = request.payload.Candidate.Id;
                var result = await _candidateService.DeleteCandidate(candidate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = HandleException(ex);
                return result;
            }
        }
    }
}
