using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.Infrastructure;
using Recruiter.API.Services;
using Microsoft.EntityFrameworkCore;

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
                options.UseSqlServer(configuration.GetConnectionString("ManagementConnection"),
                    providerOptions => providerOptions.CommandTimeout(60));
            });
            return services;
        }


        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IRecruiterService, RecruiterService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICryptoService, CryptoService>();
        }
    }
}
