using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruiter.API.Common.Constants;
using Recruiter.API.Common.Exceptions;

namespace Recruiter.API.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BaseApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [NonAction]
        public ObjectResult HandleException(Exception exception)
        {
            ObjectResult customResult = null;
            var statusCode = HttpStatusCode.Status500InternalServerError;
            var message = ExceptionMessage.RealtimeError;
            switch (exception)
            {
                case AuthorizedException ex:
                    // incase need custom error
                    //var dataError = new { Message = exception.Message, IsBusinessError = true };
                    //customResult = new ObjectResult(new { statusCode = StatusCodes.Status200OK, message = exception.Message, currentDate DateTime.UtcNow, //data = dataError });
                    statusCode = HttpStatusCode.Status401Unauthorized;
                    message = ExceptionMessage.UnAuthorized;
                    break;
                default:
                    break;
            }

            if (customResult != null)
            {
                return customResult;
            }

            var result = new ObjectResult(new { statusCode, message, currentDate = DateTime.UtcNow });
            return StatusCode(statusCode, result);
        }

        [NonAction]
        public ObjectResult ReturnOkResult(string message)
        {
            return new ObjectResult(new { HttpStatusCode.Status200OK, message, currentDate = DateTime.UtcNow });
        }

        [NonAction]
        public ObjectResult ReturnOkResult(object data)
        {
            return new ObjectResult(new { HttpStatusCode.Status200OK, data, currentDate = DateTime.UtcNow });
        }
    }
}
