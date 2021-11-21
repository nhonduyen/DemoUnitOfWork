using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.API.Services;
using Recruiter.API.ViewModel.Requests.User;
using Recruiter.Infrastructure.Logger;
using Microsoft.AspNetCore.Authorization;

namespace Recruiter.API.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : SecureBaseController
    {
        private readonly IUserService _userService;
        private readonly IAppLogger<UserController> _logger;

        public UserController(IUserService userService, IAppLogger<UserController> logger, IHttpContextAccessor httpContextAccessor
        ) : base(httpContextAccessor)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LoggedUser()
        {
            var message = $"Hello authorized user id {UserInfo.UserId} access token {AccessToken}";
            _logger.Log(message);
            return Ok(message);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.Log("Validate user identity");
                if (request.payload == null || string.IsNullOrEmpty(request.payload.username) || string.IsNullOrEmpty(request.payload.password))
                    return Unauthorized();
                var token = await _userService.ProcessLogin(request.payload.username, request.payload.password);
                if (token == null)
                    return Unauthorized();
                _logger.Log($"User logged in {request.payload.username.Trim()} - {DateTime.UtcNow}");
                Response.Cookies.Append("X-Access-Token", token.Token);
                return Ok(token);
            }
            catch(Exception ex)
            {
                var result = HandleException(ex);
                return result;
            }
        }
    }
}
