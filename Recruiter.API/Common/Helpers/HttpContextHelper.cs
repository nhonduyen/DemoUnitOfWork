using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Recruiter.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Recruiter.API.Common.Helpers
{
    public static class HttpContextHelper
    {
        public static bool GetHeaderValueByKey(IHttpContextAccessor httpContextAnccessor, string key, out StringValues value)
        {
            try
            {
                var canGet = GetValueByKey(httpContextAnccessor.HttpContext.Request.Headers, key, out value);
                return canGet;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static bool GetHeaderValueByKey(ActionExecutingContext context, string key, out StringValues value)
        {
            var canGet = GetValueByKey(context.HttpContext.Request.Headers, key, out value);
            return canGet;
        }

        public static bool GetValueByKey(IEnumerable<KeyValuePair<string, StringValues>> data, string key, out StringValues value)
        {
            var keyValuePair = data.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            value = keyValuePair.Value;
            return keyValuePair.Key != null;
        }

        public static string GetValueClaim(IHttpContextAccessor context, string ClaimTypes)
        {
            var user = (ClaimsPrincipal)context.HttpContext.User;
            return user.GetPartValueOfIndentityClaimWithKey(ClaimTypes);
        }
    }
}
