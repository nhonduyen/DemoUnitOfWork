using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Recruiter.API.Extensions
{
    public static class PrincipalExtensions
    {
        public static string GetPartValueOfIndentityClaimWithKey(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal?.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }
    }
}
