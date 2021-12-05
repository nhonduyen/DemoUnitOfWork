using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Recruiter.Services.Interface
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> SendPostAsync(string apiName, Uri uri, HttpContent content);
        Task<HttpResponseMessage> SendGetAsync(string apiName, Uri uri, List<KeyValuePair<string, string>> httpHeader = null);
        Task<HttpResponseMessage> SendPutAsync(string apiName, Uri uri, HttpContent content);
    }
}
