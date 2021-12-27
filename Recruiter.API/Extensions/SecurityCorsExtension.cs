using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Recruiter.API.Extensions
{
    public static class SecurityCorsExtension
    {
        static readonly ILoggerFactory loggerFactory = new LoggerFactory();
        static readonly ILogger _logger = loggerFactory.CreateLogger("SecurityCorsExtension");

        public static void UseCommonCors(this IApplicationBuilder application)
        {
            _logger.LogInformation("Begin UseCommonCors");
            application.UseCors("CorsPolicy");
            _logger.LogInformation("End UseCommonCors");
        }

        public static void AddCommonCors(this IServiceCollection services, IConfiguration configuration)
        {
            _logger.LogInformation("Begin AddCommonCors");
            var isAllowedOriginEnable = configuration.GetValue<bool>("AllowedOrigins:Enable");
            var stringUrls = configuration.GetSection("AllowedOrigins:Urls").Get<List<string>>();
            _logger.LogInformation($"isAllowedOriginEnable: {isAllowedOriginEnable}");

            if (isAllowedOriginEnable && stringUrls.Any())
            {
                var corsOriginAllowed = stringUrls.ToArray();
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.SetIsOriginAllowed((host) =>
                        {
                            var result = corsOriginAllowed.Any(origin => Regex.IsMatch(host, origin, RegexOptions.IgnoreCase));
                            return result;
                        })
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });
            }
            _logger.LogInformation("End AddCommonCors");
        }
    }
}
