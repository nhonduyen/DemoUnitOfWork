using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Infrastructure.Gateway
{
    public class ApiException : Exception
    {
        public HttpMethod RequestMethod { get; }
        public Uri RequestUri { get; }
        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
        public string ReasonPhase { get; }

        public bool HasContent
        {
            get { return !string.IsNullOrWhiteSpace(Content); }
        }

        public ApiException(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage, string contentString) : 
            this(requestMessage.Method, requestMessage.RequestUri, responseMessage.StatusCode, responseMessage.ReasonPhrase, contentString)
        { 
        }

        public ApiException(HttpMethod method, Uri requestUri, HttpStatusCode statusCode, string reasonPhrase, string contentString)
            : base($"{method}\\{requestUri}\\{contentString}\\{(int)statusCode}.")
        {
            this.RequestMethod = method;
            this.Data[nameof(this.RequestMethod)] = method.Method;
            this.RequestUri = requestUri;
            this.Data[nameof(this.RequestUri)] = requestUri;
            this.StatusCode = statusCode;
            this.Data[nameof(this.StatusCode)] = statusCode;
            ReasonPhase = reasonPhrase;
            this.Data[nameof(this.ReasonPhase)] = reasonPhrase;
            Content = contentString;
            this.Data[nameof(this.Content)] = contentString;
        }

        public static async Task<ApiException> CreateAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return new ApiException(request, response, "Fail with status code: ");
            }

            HttpContentHeaders contentHeader = null;
            string contentString = null;

            try
            {
                contentHeader = response.Content.Headers;
                using (var content = response.Content)
                {
                    contentString = await content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch { } // Don't want hide the original exeption with a new one

            return new ApiException(request, response, contentString);
        }
    }
}
