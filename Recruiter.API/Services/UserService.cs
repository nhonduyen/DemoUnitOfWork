using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Infrastructure;
using Recruiter.Core.Entities.ViewModel.Requests;
using Recruiter.Core.Entities.ViewModel;

namespace Recruiter.API.Services
{
    public class UserService : IUserService
    {
        private readonly RecruiterContext _recruiterContext;
        private readonly ICryptoService _cryptoService;
        private readonly IAuthenService _authenService;

        public UserService(RecruiterContext recruiterContext, ICryptoService cryptoService, IAuthenService authenService)
        {
            _recruiterContext = recruiterContext;
            _cryptoService = cryptoService;
            _authenService = authenService;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _recruiterContext.Repository<User>()
                .AsNoTracking()
                .Where(x => x.UserName.Equals(username) && !x.IsDeleted)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            var hashedPassword = _cryptoService.ComputeSha256Hash(password);
            var result = await _recruiterContext.Repository<User>()
                .AsNoTracking()
                .Where(x => x.UserName.Equals(username) && x.Password.Equals(hashedPassword) && !x.IsDeleted)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<TokenInfoVM> ProcessLogin(string username, string password)
        {
            var loggedUser = await Login(username, password);
            if (loggedUser == null)
                return null;

            var token = _authenService.RequestToken(loggedUser);
            return token;
        }

        public async Task<int> UpdateRefreshToken(string username, string refreshToken)
        {
            var user = await GetUserByUsername(username);
            if (user == null)
            {
                return -1;
            }

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddHours(8);

            _recruiterContext.Attach(user);
            _recruiterContext.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _recruiterContext.Entry(user).Property(x => x.RefreshTokenExpiryTime).IsModified = true;

            var result = await _recruiterContext.SaveChangesAsync();

            return result;
        }

        public async Task<int> UpdateRefreshToken(User user, string token)
        {
            if (user == null)
            {
                return -1;
            }

            user.RefreshToken = token;

            _recruiterContext.Attach(user);
            _recruiterContext.Entry(user).Property(x => x.RefreshToken).IsModified = true;

            var result = await _recruiterContext.SaveChangesAsync();

            return result;
        }
    }
}
