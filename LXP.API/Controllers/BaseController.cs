// BaseController.cs
using LXP.Common.ViewModels;
using LXP.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace LXP.Api.Controllers
{
    /// <summary>
    /// Base controller providing common functionalities and responses for API controllers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Creates a success response with optional data.
        /// </summary>
        [NonAction]
        public APIResponse CreateSuccessResponse(dynamic result = null)
        {
            return new APIResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = MessageConstants.MsgSuccess,
                Data = result
            };
        }

        /// <summary>
        /// Creates a failure response with specified message and status code.
        /// </summary>
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

        /// <summary>
        /// Creates a response for successful data insertion with optional data.
        /// </summary>
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

        /// <summary>
        /// Creates a response for successful request with no content and optional data.
        /// </summary>
        [NonAction]
        public APIResponse CreateNoContentResponse(dynamic result)
        {
            return new APIResponse()
            {
                StatusCode = (int)HttpStatusCode.NoContent,
                Message = MessageConstants.MsgNoContent,
                Data = result
            };
        }

        /// <summary>
        /// Validates the given model and returns appropriate response if validation fails.
        /// </summary>
        [NonAction]
        public IActionResult ValidateModel(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (validationResults.Any())
            {
                var errorMessage = string.Join(" | ", validationResults.Select(x => x.ErrorMessage));
                return BadRequest(CreateFailureResponse(errorMessage, (int)HttpStatusCode.BadRequest));
            }

            return null;
        }
    }
}

//using LXP.Common.ViewModels;
//using LXP.Common.Constants;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;

//namespace LXP.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BaseController : ControllerBase
//    {
//        [NonAction]
//        public APIResponse CreateSuccessResponse(dynamic result)
//        {
//            return new APIResponse()
//            {
//                StatusCode = (int)HttpStatusCode.OK,
//                Message = MessageConstants.MsgSuccess,
//                Data = result
//            };
//        }
//        [NonAction]
//        public APIResponse CreateFailureResponse(string message, int statusCode)
//        {
//            return new APIResponse()
//            {
//                StatusCode = statusCode,
//                Message = message,
//                Data = null
//            };
//        }

//        [NonAction]
//        public APIResponse CreateInsertResponse(dynamic result)
//        {
//            return new APIResponse()
//            {
//                StatusCode = (int)HttpStatusCode.Created,
//                Message = MessageConstants.MsgCreated,
//                Data = result
//            };
//        }

//        [NonAction]
//        public APIResponse CreateNoContentResponse(dynamic result)
//        {
//            return new APIResponse()
//            {
//                StatusCode = (int)HttpStatusCode.NoContent,
//                //  Message = MessageConstants.MsgCreated,
//                Data = result
//            };
//        }

//    }
//}
