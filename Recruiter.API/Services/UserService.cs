using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Domain.Model;
using Recruiter.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Recruiter.API.Services
{
    public class UserService : IUserService
    {
        private readonly RecruiterContext _recruiterContext;
        private readonly ICryptoService _cryptoService;
        private readonly IAuthenService _authenService;
        private readonly IConfiguration _configuration;

        public UserService(RecruiterContext recruiterContext, ICryptoService cryptoService, IAuthenService authenService, IConfiguration configuration)
        {
            _recruiterContext = recruiterContext;
            _cryptoService = cryptoService;
            _authenService = authenService;
            _configuration = configuration;
        }
        public async Task<User> Login(string username, string password)
        {
            var hashedPassword = _cryptoService.ComputeSha256Hash(password);
            var result = await _recruiterContext.Repository<User>()
                .AsNoTracking()
                .Where(x => x.UserName.Equals(username) && x.Password.Equals(hashedPassword) && !x.IsDeleted)
                .Select(x => new User
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    DepartmentId = x.DepartmentId
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<TokenInfo> ProcessLogin(string username, string password)
        {
            var loggedUser = await Login(username, password);
            if (loggedUser == null)
                return null;
            var token = _authenService.RequestToken(loggedUser, _configuration);
            return token;
        }
    }
}
