using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static FrontEnd.Gateway.Extensions.RestExtension.RestOptions;

namespace FrontEnd.Gateway.Infrastructure.NavigateService
{
    public interface INavigateService
    {
        Task<HttpResponseMessage> RequestWithoutDeserializeAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null);
        Task<HttpResponseMessage> RequestAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null);
        Task<HttpResponseMessage> RequestStreamAsync(string uri, object item, bool readBody, string nameHttpClient, bool formatResponse = false, string httpMethod = null);
        Task<string> NavigateApi(string serviceName, string endpoint = "");
        Task<HttpResponseMessage> RequestFileAsync(string uri, object requestInfo, string nameHttpClient);
        List<Service> GetServices();
        Service GetService(string serviceName);
    }
}
