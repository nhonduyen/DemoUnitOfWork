using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recruiter.Infrastructure;
using Recruiter.API.Services;
using Microsoft.EntityFrameworkCore;
using Recruiter.Infrastructure.Logger;
using Microsoft.Extensions.Logging;
using Recruiter.API.Service;
using Recruiter.Services.Interface;
using Recruiter.Services.Implement;
using Recruiter.Infrastructure.Repositories.Implements;

namespace Recruiter.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IContext, RecruiterContext>();
            services.AddScoped(typeof(IUnitOfWorkGeneric<>), typeof(UnitOfWorkGeneric<>));
            services.AddDbContext<RecruiterContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ManagementConnection"), providerOptions => providerOptions.CommandTimeout(120));
            });
            return services;
        }

        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            return services
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICandidateService, CandidateService>()
                .AddScoped<IRecruiterService, RecruiterService>()
                .AddScoped<ICandidateService, CandidateService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICryptoService, CryptoService>()
                .AddScoped<IAuthenService, AuthenService>()
                .AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>))
                .AddScoped<IHttpClientService, HttpClientService>();
        }
    }
}
