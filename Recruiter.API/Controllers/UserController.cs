using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Recruiter.API.Services;
using Recruiter.Infrastructure.Logger;
using Microsoft.AspNetCore.Authorization;
using Recruiter.Core.Controllers;
using Recruiter.Core.Entities.ViewModel.Requests.User;
using System.Linq;
using Recruiter.Infrastructure;
using Recruiter.Core.Entities.ViewModel.Responses.User;
using Recruiter.Core.Entities.DbModel;

namespace Recruiter.API.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : SecureBaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthenService _authenService;
        private readonly IAppLogger<UserController> _logger;
        private readonly IUnitOfWorkGeneric<RecruiterContext> _recruiterUow;

        public UserController(
            IUserService userService, IAuthenService authenService,
            IAppLogger<UserController> logger,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWorkGeneric<RecruiterContext> recruiterUow
        ) : base(httpContextAccessor)
        {
            _userService = userService;
            _logger = logger;
            _authenService = authenService;
            _recruiterUow = recruiterUow;
        }

        [HttpGet]
        public IActionResult LoggedUser()
        {
            var message = $"Hello authorized user id {UserInfo.UserId} username {UserInfo.UserName} access token {AccessToken}";
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

                token.RefreshToken = _authenService.GenerateRefreshToken();
                await _userService.UpdateRefreshToken(request.payload.username, token.RefreshToken);

                _logger.Log($"User logged in {request.payload.username.Trim()} - {DateTime.UtcNow}");
                Response.Cookies.Append("X-Access-Token", token.Token);
                Response.Cookies.Append("X-Refresh-Token", token.RefreshToken);
                return Ok(token);
            }
            catch(Exception ex)
            {
                var result = HandleException(ex);
                return result;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (request is null)
                {
                    return BadRequest("Invalid client request");
                }
                var accessToken = request.payload.accessToken;
                var refreshToken = request.payload.RefreshToken;

                var principal = _authenService.GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Identity.Name;

                var user = await _userService.GetUserByUsername(username);

                if (user == null || !user.RefreshToken.Trim().Equals(refreshToken) || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Invalid client request");
                }

                var tokenInfoVM = _authenService.RequestToken(user);
                var newAccessRefreshToken = _authenService.GenerateRefreshToken();
                await _userService.UpdateRefreshToken(user, newAccessRefreshToken);

                var result = new GetRefreshTokenResult
                {
                    Data = new GetRefreshTokenResultData
                    {
                        AccessToken = tokenInfoVM.Token,
                        RefreshToken = newAccessRefreshToken
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = HandleException(ex);
                return result;
            }
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Revoke()
        {
            try
            {
                var username = User.Identity.Name;

                await _userService.UpdateRefreshToken(username, null);

                return NoContent();
            }
            catch (Exception ex)
            {
                var result = HandleException(ex);
                return result;
            }
        }
    }
}
