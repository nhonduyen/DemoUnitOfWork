using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Recruiter.API.Services;
using Recruiter.Core.Entities.ViewModel.Requests.Candidate;
using Recruiter.Services.Interface;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;

namespace Recruiter.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;
        private readonly IHttpClientService _httpClientService;
        public RecruiterController(IRecruiterService recruiterService, IHttpClientService httpClientService)
        {
            _recruiterService = recruiterService;
            _httpClientService = httpClientService;
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

        [HttpGet]
        public async Task<IActionResult> GetDepartmentFromManagement(Guid id)
        {
            var result = await _httpClientService.SendGetAsync("Management", new Uri($"https://localhost:5001/Department/GetDepartmentById?id={id}"));
            var response = await result.Content.ReadAsStringAsync();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> AddDepartmentFromManagement()
        {
            var department = new
            {
                Id =Guid.NewGuid(),
                DepartmentName = $"department_{Guid.NewGuid():N}",
                IsDeleted = false,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(department), Encoding.UTF8, "application/json");
            var result = await _httpClientService.SendPostAsync("Management", new Uri($"https://localhost:5001/Department/AddDepartment"), stringContent);
            var response = await result.Content.ReadAsStringAsync();
            return Ok(response);
        }

    }
}
