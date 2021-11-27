using Microsoft.Extensions.Configuration;
using Recruiter.Core.Entities.DbModel;
using Recruiter.Core.Entities.ViewModel;
using Recruiter.Core.Entities.ViewModel.Requests;

namespace Recruiter.API.Services
{
    public interface IAuthenService
    {
        public TokenInfoVM RequestToken(User user, IConfiguration configuration);
    }
}
