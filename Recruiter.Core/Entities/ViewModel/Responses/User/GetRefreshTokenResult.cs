using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Responses.User
{
    public class GetRefreshTokenResult
    {
        public GetRefreshTokenResultData Data { get; set; }

        public GetRefreshTokenResult()
        {
            Data = new GetRefreshTokenResultData();
        }
    }

    public class GetRefreshTokenResultData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
