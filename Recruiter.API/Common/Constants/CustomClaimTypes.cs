using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruiter.API.Common.Constants
{
    public class CustomClaimTypes
    {
        public const string Username = "x-username";
        public const string Email = "x-email";
        public const string ApiAccessToken = "ApiAccessToken";
        public const string ApiRefreshToken = "ApiRefreshToken";
        public const string TokenType = "TokenType";
        public const string ExpiresIn = "ExpiresIn";
        public const string LoginTime = "LoginTime";
        public const string FlagLogout = "FlagLogout";
        public const string ForceLogoutExpire = "ForceLogoutExpire";
        public const string UserId = "x-userId";
        public const string SessionId = "x-sessionId";
    }
}
