using FrontEnd.Gateway.Extensions;
using FrontEnd.Gateway.Extensions.RestExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using static FrontEnd.Gateway.Extensions.RestExtension.RestOptions;
using Newtonsoft.Json;
using System.Text;

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

        public Service GetService(string serviceName)
        {
            var services = GetServices();
            return services.SingleOrDefault(x => x.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
        }

        public List<Service> GetServices()
        {
            return _restSettings.GetOptions<List<Service>>("ApiResources");
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

        /*private Task<HttpResponse> SendRequestAsync<T>(string uri, T item, string serviceName, bool readBody = true, bool formatResponse = false, string httpMethod = null)
        {
            try
            {
                var message = string.Empty;

            }
            catch (Exception ex)
            {
                var errorCode = "error";
                var response = new { status = new { code = errorCode, msg = ex.Message } };

                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
                };
                return message;
            }
        }*/
    }
}
