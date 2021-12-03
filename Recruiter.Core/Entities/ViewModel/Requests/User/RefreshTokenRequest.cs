using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Requests.User
{
    public class RefreshTokenRequest
    {
        public RefreshTokenRequestPayload payload { get; set; }
    }

    public class RefreshTokenRequestPayload
    {
        public string accessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
