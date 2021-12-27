using FrontEnd.Gateway.Common.Enums;
using FrontEnd.Gateway.Infrastructure.Gateway;
using FrontEnd.Gateway.Infrastructure.NavigateService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RestController : ControllerBase
    {
        private readonly INavigateService _navigateService;
        private readonly ILogger _logger;

        public RestController(INavigateService navigateService, ILogger<RestController> logger)
        {
            _navigateService = navigateService;
            _logger = logger;
        }

        [HttpGet("recruiter/getCandidates")]
        public async Task<IActionResult> GetCandidates()
        {
            var result = await Route(ApiName.RECRUITER.ToString(), "recruiter/getCandidates", null);
            return Ok(result);
        }

        [HttpPost("{serviceName}/{*endpoint}")]
        public async Task<IActionResult> PostBaseAsync(string serviceName, string endpoint, [FromBody] object requestMessage)
        {
            string uri = string.Empty;

            try
            {
                _logger.LogInformation($"Time start call api {endpoint} : {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff tt")}");
                uri = await _navigateService.NavigateApi(serviceName, endpoint);
                if (string.IsNullOrEmpty(uri)) return BadRequest();

                var response = await _navigateService.RequestWithoutDeserializeAsync(uri, requestMessage, true, serviceName);
                _logger.LogInformation($"Time end call api {endpoint} : {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff tt")}");
                return new ObjectResult(await response.Content.ReadAsStreamAsync());
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Call url {endpoint} error {ex.Message}", ex);
                var errorCode = "error";
                var response = new { status = new { code = errorCode, msg = ex.Message } };
                var message = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                return new JsonResult(message);
                throw;
            }
        }

        private async Task<IActionResult> Route(string serviceName, string endpoint, object value)
        {
            try
            {
                var uri = await _navigateService.NavigateApi(serviceName, endpoint);
                if (string.IsNullOrWhiteSpace(uri)) return BadRequest();

                var result = await _navigateService.RequestAsync<object>(uri, value, false, serviceName, true);
                return new JsonResult(result);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Call url {endpoint} error {ex.Message}", ex);
                var errorCode = "error";
                var response = new { status = new { code = errorCode, msg = ex.Message } };
                var message = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                return new JsonResult(message);
            }
        }
    }
}
