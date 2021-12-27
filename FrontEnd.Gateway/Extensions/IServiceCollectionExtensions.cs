using FrontEnd.Gateway.Infrastructure.HttpClientService;
using FrontEnd.Gateway.Infrastructure.NavigateService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<INavigateService, NavigateService>()
                .AddScoped<IHttpClientService, HttpClientService>();
        }
    }
}
