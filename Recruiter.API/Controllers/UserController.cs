using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.API.Services;
using Recruiter.API.ViewModel.Requests.User;

namespace Recruiter.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.payload == null || string.IsNullOrEmpty(request.payload.username) || string.IsNullOrEmpty(request.payload.password))
                return Unauthorized();
            var loggedInUser = await _userService.Login(request.payload.username, request.payload.password);
            if (loggedInUser == null)
                return Unauthorized();
            return Ok(loggedInUser);
        }
    }
}
