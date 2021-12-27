using FrontEnd.Gateway.Extensions;
using FrontEnd.Gateway.Extensions.RestExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using static FrontEnd.Gateway.Extensions.RestExtension.RestOptions;
using Newtonsoft.Json;
using System.Text;
using FrontEnd.Gateway.Infrastructure.HttpClientService;
using FrontEnd.Gateway.Infrastructure.Gateway;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace FrontEnd.Gateway.Infrastructure.NavigateService
{
    public class NavigateService : INavigateService
    {
        private readonly IConfiguration _config;
        private readonly IConfigurationSection _restSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<NavigateService> _logger;
        private readonly IHttpClientService _httpClientService;
        private readonly string TokenLog;

        public NavigateService(IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            ILogger<NavigateService> logger,
            IHttpClientService httpClientService)
        {
            _config = config;
            _restSettings = _config.GetSection("ApiResources");
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _httpClientService = httpClientService;
            TokenLog = Guid.NewGuid().ToString();
        }

        public Service GetService(string serviceName)
        {
            var services = GetServices();
            return services.SingleOrDefault(x => x.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
        }

        public List<Service> GetServices()
        {
            return _restSettings.GetOptions<List<Service>>("ApiNames");
        }

        public async Task<string> NavigateApi(string serviceName, string endpoint = "")
        {
            try
            {
                var service = GetService(serviceName);
                if (service == null) return string.Empty;

                var uri = service.Uri;
                var host = $"{uri}{endpoint}{_httpContextAccessor.HttpContext.Request.QueryString.Value}";
                var result = await Task.FromResult(host);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<T> RequestAsync<T>(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null)
        {
            var response = await SendRequestAsync(uri, item, nameHttpClient, readBody, formatResponse, httpMethod);

            if (response.Headers.Where(x => x.Key == "ETag").Any())
            {
                _httpContextAccessor.HttpContext.Response.Headers.Add("ETag", response.Headers.Where(x => x.Key == "ETag").FirstOrDefault().Value.ToString());
            }

            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                using (var reader = new StreamReader(stream))
                {
                   using (var jsonReader = new JsonTextReader(reader))
                    {
                        var ser = new JsonSerializer();
                        return ser.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        public async Task<HttpResponseMessage> RequestFileAsync(string uri, object requestInfo, string nameHttpClient)
        {
            return await SendRequestAsync(uri, requestInfo, nameHttpClient);
        }

        public async Task<HttpResponseMessage> RequestStreamAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false)
        {
            return await this.SendRequestStreamAsync(uri, item, nameHttpClient, readBody, formatResponse);
        }

        public async Task<HttpResponseMessage> RequestWithoutDeserializeAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null)
        {
            var response = await SendRequestAsync(uri, item, nameHttpClient, readBody, formatResponse, httpMethod);

            if (response.Headers.Where(x => x.Key == "ETag").Any())
            {
                _httpContextAccessor.HttpContext.Response.Headers.Add("ETag", response.Headers.Where(x => x.Key == "ETag").FirstOrDefault().Value.ToString());
            }

            return response;
        }

        private async Task<HttpResponseMessage> SendRequestAsync<T>(string uri, T item, string serviceName, bool readBody = true, bool formatResponse = false, string httpMethod = null)
        {
            try
            {
                var message = string.Empty;
                var requestMessage = new HttpRequestMessage(new HttpMethod(string.IsNullOrEmpty(httpMethod) ? _httpContextAccessor.HttpContext.Request.Method : httpMethod),
                    new Uri(uri));
                requestMessage.AddAuthorization(_httpContextAccessor);
                await requestMessage.Content(_httpContextAccessor, TokenLog, _logger, item);

                var response = await _httpClientService.SendAsync(serviceName, requestMessage).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode)
                {
                    message = $"Result status code: {TokenLog} - {requestMessage.RequestUri.AbsoluteUri} - {response.StatusCode}";
                    _logger.LogError(message);
                    throw await ApiException.CreateAsync(requestMessage, response).ConfigureAwait(false);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorCode = "error";
                var response = new { status = new { code = errorCode, msg = ex.Message } };
                var logMessage = $"Result status code: {TokenLog} - {_httpContextAccessor.HttpContext.Request.Headers["Path"]} - {ex.Message} - {ex.StackTrace}";
                _logger.LogCritical(logMessage);

                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                return message;
            }
        }

        private async Task<HttpResponseMessage> SendRequestStreamAsync<T>(string uri, T item, string serviceName, bool readBody = true, bool formatResponse = false)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod(_httpContextAccessor.HttpContext.Request.Method), new Uri(uri));
            return await _httpClientService.SendAsync(serviceName, requestMessage).ConfigureAwait(false);
        }
    }
}
