using LXP.Common.Constants;
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicFeedbackController : BaseController
    {
        private readonly ITopicFeedbackService _service;

        public TopicFeedbackController(ITopicFeedbackService service)
        {
            _service = service;
        }

        ///<summary>
        /// Add a new feedback question.
        ///</summary>
        ///<param name="question">The feedback question to add.</param>
        ///<response code="200">Question added successfully.</response>
        ///<response code="400">Question object is null.</response>
        ///<response code="500">Failed to add question.</response>
        [HttpPost("question")]
        public IActionResult AddFeedbackQuestion(TopicFeedbackQuestionViewModel question)
        {
            if (question == null)
            {
                return BadRequest(CreateFailureResponse("Question object is null", 400));
            }

            var options = question.Options ?? new List<TopicFeedbackQuestionsOptionViewModel>(); // Ensure options are not null
            var questionId = _service.AddFeedbackQuestion(question, options);

            if (questionId != Guid.Empty)
            {
                return Ok(CreateSuccessResponse("Question added successfully"));
            }

            return StatusCode(500, CreateFailureResponse("Failed to add question", 500));
        }

        ///<summary>
        /// Retrieve all feedback questions.
        ///</summary>
        ///<response code="200">List of all feedback questions.</response>
        ///<response code="500">Internal server error.</response>
        [HttpGet]
        public IActionResult GetAllFeedbackQuestions()
        {
            var questions = _service.GetAllFeedbackQuestions();
            return Ok(CreateSuccessResponse(questions));
        }

        ///<summary>
        /// Retrieve a feedback question by its ID.
        ///</summary>
        ///<param name="topicFeedbackQuestionId">The ID of the feedback question.</param>
        ///<response code="200">Feedback question details.</response>
        ///<response code="404">Feedback question not found.</response>
        ///<response code="500">Internal server error.</response>
        [HttpGet("{topicFeedbackQuestionId}")]
        public IActionResult GetFeedbackQuestionById(Guid topicFeedbackQuestionId)
        {
            var question = _service.GetFeedbackQuestionById(topicFeedbackQuestionId);
            if (question == null)
            {
                return NotFound(CreateFailureResponse("Feedback question not found", 404));
            }

            return Ok(CreateSuccessResponse(question));
        }

        ///<summary>
        /// Update an existing feedback question.
        ///</summary>
        ///<param name="topicFeedbackQuestionId">The ID of the feedback question to update.</param>
        ///<param name="question">The updated feedback question.</param>
        ///<response code="200">Feedback question updated successfully.</response>
        ///<response code="400">Question object is null.</response>
        ///<response code="404">Feedback question not found.</response>
        ///<response code="500">Failed to update feedback question.</response>
        [HttpPut("{topicFeedbackQuestionId}")]
        public IActionResult UpdateFeedbackQuestion(
            Guid topicFeedbackQuestionId,
            TopicFeedbackQuestionViewModel question
        )
        {
            var existingQuestion = _service.GetFeedbackQuestionById(topicFeedbackQuestionId);
            if (existingQuestion == null)
            {
                return NotFound(CreateFailureResponse("Feedback question not found", 404));
            }

            var options = question.Options ?? new List<TopicFeedbackQuestionsOptionViewModel>(); // Ensure options are not null
            var result = _service.UpdateFeedbackQuestion(
                topicFeedbackQuestionId,
                question,
                options
            );

            if (result)
            {
                return Ok(CreateSuccessResponse("Feedback question updated successfully"));
            }

            return StatusCode(
                500,
                CreateFailureResponse("Failed to update feedback question", 500)
            );
        }

        ///<summary>
        /// Delete a feedback question.
        ///</summary>
        ///<param name="topicFeedbackQuestionId">The ID of the feedback question to delete.</param>
        ///<response code="200">Feedback question deleted successfully.</response>
        ///<response code="404">Feedback question not found.</response>
        ///<response code="500">Failed to delete feedback question.</response>
        [HttpDelete("{topicFeedbackQuestionId}")]
        public IActionResult DeleteFeedbackQuestion(Guid topicFeedbackQuestionId)
        {
            var existingQuestion = _service.GetFeedbackQuestionById(topicFeedbackQuestionId);
            if (existingQuestion == null)
            {
                return NotFound(CreateFailureResponse("Feedback question not found", 404));
            }

            var result = _service.DeleteFeedbackQuestion(topicFeedbackQuestionId);

            if (result)
            {
                return Ok(CreateSuccessResponse("Feedback question deleted successfully"));
            }

            return StatusCode(
                500,
                CreateFailureResponse("Failed to delete feedback question", 500)
            );
        }

        ///<summary>
        /// Retrieve feedback questions by topic ID.
        ///</summary>
        ///<param name="topicId">The ID of the topic.</param>
        ///<response code="200">List of feedback questions for the specified topic.</response>
        ///<response code="404">No questions found for the given topic.</response>
        ///<response code="500">Internal server error.</response>
        [HttpGet("topic/{topicId}")]
        public IActionResult GetFeedbackQuestionsByTopicId(Guid topicId)
        {
            var questions = _service.GetFeedbackQuestionsByTopicId(topicId);
            if (questions == null || !questions.Any())
            {
                return NotFound(
                    CreateFailureResponse("No questions found for the given topic", 404)
                );
            }

            return Ok(CreateSuccessResponse(questions));
        }
    }
}
