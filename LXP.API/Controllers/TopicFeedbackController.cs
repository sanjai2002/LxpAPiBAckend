using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicFeedbackController : ControllerBase
    {
        private readonly ITopicFeedbackService _service;

        public TopicFeedbackController(ITopicFeedbackService service)
        {
            _service = service;
        }

        [HttpPost("question")]
        public IActionResult AddFeedbackQuestion(TopicFeedbackQuestionDTO question)
        {
            if (question == null)
                return BadRequest("Question object is null");

            var result = _service.AddFeedbackQuestion(question, question.Options);

            if (result)
                return Ok("Question added successfully");

            return BadRequest("Failed to add question");
        }

        [HttpGet]
        public IActionResult GetAllFeedbackQuestions()
        {
            var questions = _service.GetAllFeedbackQuestions();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public IActionResult GetFeedbackQuestionById(Guid id)
        {
            var question = _service.GetFeedbackQuestionById(id);
            if (question == null)
                return NotFound();

            return Ok(question);
        }

        [HttpPost("response")]
        public IActionResult SubmitFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse)
        {
            _service.SubmitFeedbackResponse(feedbackResponse);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question)
        {
            if (question == null)
                return BadRequest("Question object is null");

            var result = _service.UpdateFeedbackQuestion(id, question, question.Options);

            if (result)
                return Ok("Question updated successfully");

            return NotFound("Question not found");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFeedbackQuestion(Guid id)
        {
            var result = _service.DeleteFeedbackQuestion(id);

            if (result)
                return Ok("Question deleted successfully");

            return NotFound("Question not found");
        }

        [HttpGet("topic/{topicId}")]
        public IActionResult GetFeedbackQuestionsByTopicId(Guid topicId)
        {
            var questions = _service.GetFeedbackQuestionsByTopicId(topicId);
            if (questions == null || !questions.Any())
                return NotFound("No questions found for the given topic");

            return Ok(questions);
        }

    }
}
