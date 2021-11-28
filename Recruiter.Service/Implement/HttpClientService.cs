using Recruiter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Recruiter.Services.Implement
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendGetAsync(string apiName, Uri uri, List<KeyValuePair<string, string>> httpHeader = null)
        {
            using (var httpClient = _httpClientFactory.CreateClient(apiName))
            {
                var httpRequest = new HttpRequestMessage()
                {
                    RequestUri = uri,
                    Method = HttpMethod.Get
                };
                if (httpHeader == null) return await httpClient.SendAsync(httpRequest);

                foreach (var header in httpHeader)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }

                return await httpClient.SendAsync(httpRequest);
            }
        }

        public async Task<HttpResponseMessage> SendPostAsync(string apiName, Uri uri, HttpContent content)
        {
            using (var httpClient = _httpClientFactory.CreateClient(apiName))
            {
                var httpRequest = new HttpRequestMessage()
                {
                    RequestUri = uri,
                    Method = HttpMethod.Post,
                    Content = content
                };
                return await httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            }
        }

        public Task<HttpResponseMessage> SendPutAsync(string apiName, Uri uri, HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}
