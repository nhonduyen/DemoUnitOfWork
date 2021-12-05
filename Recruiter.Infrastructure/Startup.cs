using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recruiter.Infrastructure.Repositories.Implements;
using Recruiter.Infrastructure.Repositories.Interfaces;
using Recruiter.Infrastructure.UnitOfWork;

namespace Recruiter.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection CreateDefaultDbContext(this IServiceCollection services)
        {
            services.AddScoped<IContext, RecruiterContext>();
            services.AddScoped(typeof(IUnitOfWorkGeneric<>), typeof(UnitOfWorkGeneric<>));
            services.AddDbContext<RecruiterContext>(options => options.UseInMemoryDatabase(databaseName: "Recruiter" + Guid.NewGuid()));
            return services;
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, Recruiter.Infrastructure.UnitOfWork.UnitOfWork>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
        }
    }
}
