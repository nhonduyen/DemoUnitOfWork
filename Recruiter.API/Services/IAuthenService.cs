using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Recruiter.Domain.Model;

namespace Recruiter.API.Services
{
    public interface IAuthenService
    {
        public TokenInfo RequestToken(User user, IConfiguration configuration);
    }
}
