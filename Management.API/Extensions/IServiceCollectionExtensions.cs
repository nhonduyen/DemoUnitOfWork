using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Management.Domain.Interfaces;
using Management.Domain.Departments;
using Management.Infrastructure.Repositories;
using Management.Domain.Users;
using Management.Domain.Salaries;
using Management.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Management.API.Services;
using System;

namespace Management.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Infrastructure.AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ManagementConnection"),
                    providerOptions => providerOptions.CommandTimeout(60));
            });
            services.AddScoped<Func<Infrastructure.AppDbContext>>((provider) => () => provider.GetService<Infrastructure.AppDbContext>());
            services.AddScoped<AppDbFactory>();
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>))
                .AddScoped(typeof(IDepartmentRepository), typeof(DepartmentRepository))
                .AddScoped(typeof(IUserRepository), typeof(UserRepository))
                .AddScoped(typeof(ISalaryRepository), typeof(SalaryRepository));
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<DepartmentService>();
        }
    }
}
