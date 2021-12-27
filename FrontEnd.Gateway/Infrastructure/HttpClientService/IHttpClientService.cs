using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Infrastructure.HttpClientService
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> SendAsync(string apiName, HttpRequestMessage requestMessage);
    }
}
