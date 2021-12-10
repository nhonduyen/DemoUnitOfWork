using FrontEnd.Gateway.Extensions.RestExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Infrastructure.NavigateService
{
    public class NavigateService : INavigateService
    {
        private readonly IConfiguration _config;
        private readonly IConfigurationSection _restSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public NavigateService(IConfiguration config,
            IConfigurationSection restSettings,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger)
        {
            _config = config;
            _restSettings = restSettings;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public RestOptions.Service GetService(string serviceName)
        {
            throw new NotImplementedException();
        }

        public List<RestOptions.Service> GetServices()
        {
            throw new NotImplementedException();
        }

        public Task<string> NavigateApi(string serviceName, string endpoint = "")
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RequestAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RequestFileAsync(string uri, object requestInfo, string nameHttpClient)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RequestStreamAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RequestWithoutDeserializeAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null)
        {
            throw new NotImplementedException();
        }
    }
}
