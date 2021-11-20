using System;

namespace Recruiter.Domain.Model
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
