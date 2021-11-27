using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Recruiter.Core.Common.Constants;
using Recruiter.Core.Entities.ViewModel;

namespace Recruiter.Core.Controllers
{
    public class SecureBaseController : BaseApiController
    {
        protected UserInfoVM UserInfo { get; private set; }

        public SecureBaseController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            UserInfo = GetCurrentUserFromClaim();
        }

        [NonAction]
        protected Guid? GetUserIdAuthentication(HttpContext httpContext)
        {
            StringValues authorization = AccessToken;
            if (authorization.Count > 0)
            {
                const string bearerIndex = "bearer ";
                var indexToken = authorization.First().ToLower().IndexOf(bearerIndex, StringComparison.Ordinal);
                if (indexToken == 0)
                {
                    var token = authorization.First().Substring(bearerIndex.Length);
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken tokenS = handler.ReadToken(token) as JwtSecurityToken;
                    return Guid.Parse(tokenS.Payload[CustomClaimTypes.UserId].ToString());
                }
            }
            return null;
        }

        [NonAction]
        protected UserInfoVM GetCurrentUserFromClaim()
        {
            var currentUser = new UserInfoVM();
            StringValues authorization = AccessToken;
            if (!string.IsNullOrEmpty(authorization))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken tokenS = handler.ReadToken(authorization) as JwtSecurityToken;
                currentUser.UserId = Guid.Parse(tokenS.Payload[CustomClaimTypes.UserId].ToString());
                currentUser.UserName = tokenS.Payload[CustomClaimTypes.Username].ToString();
            }
            return currentUser;
        }

        protected string AccessToken
        {
            get
            {
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorization);
                var token = string.Empty;
                if (authorization.Count > 0)
                {
                    const string bearerIndex = "bearer ";
                    var indexToken = authorization.First().ToLower().IndexOf(bearerIndex, StringComparison.Ordinal);
                    if (indexToken == 0)
                    {
                        token = authorization.First().Substring(bearerIndex.Length);
                    }
                }
                else
                {
                    token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

                    if (string.IsNullOrEmpty(token) && _httpContextAccessor.HttpContext.Request.Query.TryGetValue("token", out var tokenInQs))
                    {
                        token = tokenInQs;
                    }
                }
                return token;
            }
        }
    }
}
