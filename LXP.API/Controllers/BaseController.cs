using LXP.Common.ViewModels;
using LXP.Common.Constants;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public APIResponse CreateSuccessResponse(dynamic result=null)
        {
            return new APIResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = MessageConstants.MsgSuccess,
                Data = result
            };
        }

        [NonAction]
        public APIResponse CreateFailureResponse(string message, int statusCode)
        {
            return new APIResponse()
            {
                StatusCode = statusCode,
                Message = message,
                Data = null
            };
        }

        [NonAction]
        public APIResponse CreateInsertResponse(dynamic result)
        {
            return new APIResponse()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = MessageConstants.MsgCreated,
                Data = result
            };
        }

        [NonAction]
        public APIResponse CreateNoContentResponse(dynamic result)
        {
            return new APIResponse()
            {
                StatusCode = (int)HttpStatusCode.NoContent,
              //  Message = MessageConstants.MsgCreated,
                Data = result
            };
        }
    }
}
