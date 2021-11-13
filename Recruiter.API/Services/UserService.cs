using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Domain.Model;
using Recruiter.Infrastructure;

namespace Recruiter.API.Services
{
    public class UserService : IUserService
    {
        private readonly RecruiterContext _recruiterContext;
        private readonly ICryptoService _cryptoService;

        public UserService(RecruiterContext recruiterContext, ICryptoService cryptoService)
        {
            _recruiterContext = recruiterContext;
            _cryptoService = cryptoService;
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
    }
}
