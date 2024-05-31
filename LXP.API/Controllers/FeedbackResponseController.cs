using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Net;
using LXP.Api.Controllers;

namespace LXP.API.Controllers
{
    /// <summary>
    /// Manages feedback response operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackResponseController : BaseController
    {
        private readonly IFeedbackResponseService _feedbackResponseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackResponseController"/> class.
        /// </summary>
        /// <param name="feedbackResponseService">The feedback response service.</param>
        public FeedbackResponseController(IFeedbackResponseService feedbackResponseService)
        {
            _feedbackResponseService = feedbackResponseService;
        }

        /// <summary>
        /// Adds a new quiz feedback response.
        /// </summary>
        /// <param name="feedbackResponse">The quiz feedback response model.</param>
        /// <returns>A response indicating the result of the feedback submission.</returns>
        /// <response code="201">Quiz feedback response added successfully.</response>
        /// <response code="400">Bad request due to invalid input.</response>
        [HttpPost("AddQuizFeedbackResponse")]
        public IActionResult AddQuizFeedbackResponse([FromBody] QuizFeedbackResponseViewModel feedbackResponse)
        {
            var validationResponse = ValidateModel(feedbackResponse);
            if (validationResponse != null) return validationResponse;

            try
            {
                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
                return Ok(CreateInsertResponse("Quiz feedback response added successfully."));
            }
            catch (ValidationException ex)
            {
                return BadRequest(CreateFailureResponse(string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage)), (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(CreateFailureResponse(ex.Message, (int)HttpStatusCode.BadRequest));
            }
        }

        /// <summary>
        /// Adds a new topic feedback response.
        /// </summary>
        /// <param name="feedbackResponse">The topic feedback response model.</param>
        /// <returns>A response indicating the result of the feedback submission.</returns>
        /// <response code="201">Topic feedback response added successfully.</response>
        /// <response code="400">Bad request due to invalid input.</response>
        [HttpPost("AddTopicFeedbackResponse")]
        public IActionResult AddTopicFeedbackResponse([FromBody] TopicFeedbackResponseViewModel feedbackResponse)
        {
            var validationResponse = ValidateModel(feedbackResponse);
            if (validationResponse != null) return validationResponse;

            try
            {
                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
                return Ok(CreateInsertResponse("Topic feedback response added successfully."));
            }
            catch (ValidationException ex)
            {
                return BadRequest(CreateFailureResponse(string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage)), (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(CreateFailureResponse(ex.Message, (int)HttpStatusCode.BadRequest));
            }
        }
    }
}

//using LXP.Common.ViewModels.FeedbackResponseViewModel;
//using LXP.Services.IServices;
//using Microsoft.AspNetCore.Mvc;
//using FluentValidation;
//using System.Net;
//using LXP.Api.Controllers;

//namespace LXP.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class FeedbackResponseController : BaseController
//    {
//        private readonly IFeedbackResponseService _feedbackResponseService;

//        public FeedbackResponseController(IFeedbackResponseService feedbackResponseService)
//        {
//            _feedbackResponseService = feedbackResponseService;
//        }

//        [HttpPost("AddQuizFeedbackResponse")]
//        public IActionResult AddQuizFeedbackResponse([FromBody] QuizFeedbackResponseViewModel feedbackResponse)
//        {
//            var validationResponse = ValidateModel(feedbackResponse);
//            if (validationResponse != null) return validationResponse;

//            try
//            {
//                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
//                return Ok(CreateInsertResponse("Quiz feedback response added successfully."));
//            }
//            catch (ValidationException ex)
//            {
//                return BadRequest(CreateFailureResponse(string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage)), (int)HttpStatusCode.BadRequest));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(CreateFailureResponse(ex.Message, (int)HttpStatusCode.BadRequest));
//            }
//        }

//        [HttpPost("AddTopicFeedbackResponse")]
//        public IActionResult AddTopicFeedbackResponse([FromBody] TopicFeedbackResponseViewModel feedbackResponse)
//        {
//            var validationResponse = ValidateModel(feedbackResponse);
//            if (validationResponse != null) return validationResponse;

//            try
//            {
//                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
//                return Ok(CreateInsertResponse("Topic feedback response added successfully."));
//            }
//            catch (ValidationException ex)
//            {
//                return BadRequest(CreateFailureResponse(string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage)), (int)HttpStatusCode.BadRequest));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(CreateFailureResponse(ex.Message, (int)HttpStatusCode.BadRequest));
//            }
//        }
//    }
//}

//using LXP.Common.ViewModels.FeedbackResponseViewModel;
//using LXP.Services.IServices;
//using Microsoft.AspNetCore.Mvc;
//using FluentValidation;

//namespace LXP.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class FeedbackResponseController : ControllerBase
//    {
//        private readonly IFeedbackResponseService _feedbackResponseService;

//        public FeedbackResponseController(IFeedbackResponseService feedbackResponseService)
//        {
//            _feedbackResponseService = feedbackResponseService;
//        }

//        [HttpPost("AddQuizFeedbackResponse")]
//        public IActionResult AddQuizFeedbackResponse([FromBody] QuizFeedbackResponseViewModel feedbackResponse)
//        {
//            try
//            {
//                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
//                return Ok("Quiz feedback response added successfully.");
//            }
//            catch (ValidationException ex)
//            {
//                return BadRequest(ex.Errors);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("AddTopicFeedbackResponse")]
//        public IActionResult AddTopicFeedbackResponse([FromBody] TopicFeedbackResponseViewModel feedbackResponse)
//        {
//            try
//            {
//                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
//                return Ok("Topic feedback response added successfully.");
//            }
//            catch (ValidationException ex)
//            {
//                return BadRequest(ex.Errors);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
//using LXP.Common.ViewModels.FeedbackResponseViewModel;
//using LXP.Services.IServices;
//using Microsoft.AspNetCore.Mvc;
//using System;

//namespace LXP.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class FeedbackResponseController : ControllerBase
//    {
//        private readonly IFeedbackResponseService _feedbackResponseService;

//        public FeedbackResponseController(IFeedbackResponseService feedbackResponseService)
//        {
//            _feedbackResponseService = feedbackResponseService;
//        }

//        [HttpPost("AddQuizFeedbackResponse")]
//        public IActionResult AddQuizFeedbackResponse([FromBody] QuizFeedbackResponseViewModel feedbackResponse)
//        {
//            try
//            {
//                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
//                return Ok("Quiz feedback response added successfully.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("AddTopicFeedbackResponse")]
//        public IActionResult AddTopicFeedbackResponse([FromBody] TopicFeedbackResponseViewModel feedbackResponse)
//        {
//            try
//            {
//                _feedbackResponseService.SubmitFeedbackResponse(feedbackResponse);
//                return Ok("Topic feedback response added successfully.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}