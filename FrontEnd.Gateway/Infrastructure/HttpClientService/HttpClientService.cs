using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Infrastructure.HttpClientService
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendAsync(string apiName, HttpRequestMessage requestMessage)
        {
            using (var httpClient = _httpClientFactory.CreateClient(apiName))
            {
                return await httpClient.SendAsync(requestMessage);
            }
        }
    }
}
