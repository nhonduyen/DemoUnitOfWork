using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Domain.Model;
using Microsoft.Extensions.Primitives;
using Recruiter.API.Common.Helpers;
using Recruiter.API.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

namespace Recruiter.API.Controllers
{
    public class SecureBaseController : BaseApiController
    {
        protected TokenInfo TokenInfo { get; private set; }
        protected UserInfo UserInfo { get; private set; }

        public SecureBaseController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            UserInfo = GetCurrentUserFromClaim();
        }

        protected UserInfo GetCurrentUserFromClaim()
        {
            var currentUser = new UserInfo();
            string userIdFromClaim = HttpContextHelper.GetValueClaim(_httpContextAccessor, CustomClaimTypes.UserId);

            if (!string.IsNullOrEmpty(userIdFromClaim) && Guid.TryParse(userIdFromClaim, out Guid userId))
            {
                currentUser.UserId = userId;
            }
            return currentUser;
        }

        protected string AccessToken
        {
            get
            {
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorization);
                var token = "";
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
