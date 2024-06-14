using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackResponseDetailsController : ControllerBase
    {
        private readonly IFeedbackResponseService _feedbackResponseService;

        public FeedbackResponseDetailsController(IFeedbackResponseService feedbackResponseService)
        {
            _feedbackResponseService = feedbackResponseService;
        }

        [HttpGet("quiz/{quizId}")]
        public IActionResult GetQuizFeedbackResponses(Guid quizId)
        {
            var responses = _feedbackResponseService.GetQuizFeedbackResponses(quizId);
            return Ok(responses);
        }

        [HttpGet("topic/{topicId}")]
        public IActionResult GetTopicFeedbackResponses(Guid topicId)
        {
            var responses = _feedbackResponseService.GetTopicFeedbackResponses(topicId);
            return Ok(responses);
        }

        [HttpGet("quiz/{quizId}/learner/{learnerId}")]
        public IActionResult GetQuizFeedbackResponsesByLearner(Guid quizId, Guid learnerId)
        {
            var responses = _feedbackResponseService.GetQuizFeedbackResponsesByLearner(quizId, learnerId);
            return Ok(responses);
        }

        [HttpGet("topic/{topicId}/learner/{learnerId}")]
        public IActionResult GetTopicFeedbackResponsesByLearner(Guid topicId, Guid learnerId)
        {
            var responses = _feedbackResponseService.GetTopicFeedbackResponsesByLearner(topicId, learnerId);
            return Ok(responses);
        }
    }
}