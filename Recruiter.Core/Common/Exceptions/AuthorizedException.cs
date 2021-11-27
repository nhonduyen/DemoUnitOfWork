using System;

namespace Recruiter.Core.Common.Exceptions
{
    public class AuthorizedException : Exception
    {
        public AuthorizedException(string message) : base(message)
        {

        }
    }
}
