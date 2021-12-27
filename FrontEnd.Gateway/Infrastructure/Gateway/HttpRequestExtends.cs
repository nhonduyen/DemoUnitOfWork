using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace FrontEnd.Gateway.Infrastructure.Gateway
{
    public static class HttpRequestExtends
    {
        public static void AddAuthorization(this HttpRequestMessage requestMessage, IHttpContextAccessor httpContextAccessor)
        {
            var authorization = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var scheme = headerValue.Scheme;
                var parameter = headerValue.Parameter;
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
            }
        }
        public async static Task Content(this HttpRequestMessage requestMessage, IHttpContextAccessor httpContextAccessor, string tokenLog, ILogger logger, object item)
        {
            try
            {
                string ContentType = httpContextAccessor.HttpContext.Request.ContentType;
                var data = httpContextAccessor.HttpContext.Request;
                switch (ContentType)
                {
                    case MediaTypeNames.Application.Json:
                        if (data.ContentLength != null)
                        {
                            data.EnableBuffering();
                            data?.Body.Seek(0, SeekOrigin.Begin);
                            using (var sr = new StreamReader(data?.Body, Encoding.UTF8, true, 1024, true))
                            {
                                JObject asObject = null;
                                string value = await sr.ReadToEndAsync();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    asObject = JObject.Parse(value);
                                }
                                else
                                {
                                    asObject = JObject.FromObject(item);
                                }
                                var ttt = new StringContent(JsonConvert.SerializeObject(asObject, Formatting.Indented), Encoding.UTF8, ContentType);
                                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(asObject, Formatting.Indented), Encoding.UTF8, ContentType);
                            }
                            data.Body.Position = 0;
                        }
                        else
                        {
                            requestMessage.Content = new StringContent("{}", Encoding.UTF8, ContentType);
                        }
                        break;
                    case "application/x-www-form-urlencoded":
                        var form = (FormCollection)data.Form;
                        var encodedContent = await new FormUrlEncodedContent(form.Transform()).ReadAsStringAsync().ConfigureAwait(false);
                        requestMessage.Content = new StringContent(encodedContent, Encoding.UTF8, ContentType);
                        break;
                    default:
                        if (!string.IsNullOrEmpty(ContentType) && ContentType.Contains("multipart/form-data"))
                        {
                            var formData = new MultipartFormDataContent();
                            var files = (FormFileCollection)data.Form.Files;
                            foreach (var command in files)
                            {
                                using (var dataStream = new MemoryStream())
                                {
                                    await command.CopyToAsync(dataStream);
                                    HttpContent content = new ByteArrayContent(dataStream.ToArray());
                                    content.Headers.Add("Content-Type", command.ContentType);
                                    formData.Add(content, command.Name, command.FileName);
                                }
                            }
                            var myForm = (FormCollection)data.Form;
                            foreach (var command in myForm)
                            {
                                HttpContent content = new StringContent(command.Value, Encoding.UTF8, "application/x-www-form-urlencoded");
                                formData.Add(content, command.Key);
                            }
                            requestMessage.Content = formData;
                        }
                        break;
                }
                string msg = $"HttpRequestExtends Input: {tokenLog} - {requestMessage.Method} - {requestMessage.RequestUri}";
                logger.LogInformation(msg);
            }
            catch (Exception ex)
            {
                string msg = $"HttpRequestExtends Input: {tokenLog} - {requestMessage.Method} - {requestMessage.RequestUri} - {ex.Message} - {ex.StackTrace}";
                logger.LogCritical(msg);
                throw;
            }
        }
    }
}
