using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.API.Services;
using Recruiter.API.ViewModel.Requests.User;
using Recruiter.Infrastructure.Logger;

namespace Recruiter.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAppLogger<UserController> _logger;

        public UserController(IUserService userService, IAppLogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.Log("Validate user identity");
                if (request.payload == null || string.IsNullOrEmpty(request.payload.username) || string.IsNullOrEmpty(request.payload.password))
                    return Unauthorized();
                var loggedInUser = await _userService.Login(request.payload.username, request.payload.password);
                if (loggedInUser == null)
                    return Unauthorized();
                _logger.Log($"User logged in {loggedInUser.UserName.Trim()} - {DateTime.UtcNow}", loggedInUser);
                return Ok(loggedInUser);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
