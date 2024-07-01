using LXP.Common.ViewModels.QuizViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    /// <summary>
    /// Manages quizzes within the application.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : BaseController
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        /// <summary>
        /// Retrieves a specific quiz by its ID.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz to retrieve.</param>
        /// <response code="200">Success on finding the quiz. The response body contains a basic representation of the quiz data.</response>
        /// <response code="404">Not found if no quiz exists with the provided ID.</response>

        [HttpGet("{quizId}")]
        public IActionResult GetQuizById(Guid quizId)
        {
            var quiz = _quizService.GetQuizById(quizId);

            if (quiz == null)
                return NotFound(CreateFailureResponse($"Quiz with id {quizId} not found.", 404));

            return Ok(CreateSuccessResponse(quiz));
        }

        /// <summary>
        /// Retrieves a list of all available quizzes.
        /// </summary>
        /// <response code="200">Success. The response body contains a collection of basic quiz representations.</response>


        [HttpGet]
        public IActionResult GetAllQuizzes()
        {
            var quizzes = _quizService.GetAllQuizzes();
            return Ok(CreateSuccessResponse(quizzes));
        }

        /// <summary>
        /// Creates a new quiz.
        /// </summary>
        /// <param name="request">Data representing the new quiz to be created, provided in the request body.</param>
        /// <response code="201">Created on successful quiz creation. The response body includes a location header pointing to the newly created quiz and a basic representation of the quiz data.</response>
        /// <response code="400">Bad request due to invalid input (e.g., missing or invalid name, negative duration, etc.).</response>

        [HttpPost]
        public IActionResult CreateQuiz([FromBody] CreateQuizViewModel request)
        {
            var quiz = new QuizViewModel
            {
                QuizId = Guid.NewGuid(),
                NameOfQuiz = request.NameOfQuiz,
                Duration = request.Duration,
                PassMark = request.PassMark,
                AttemptsAllowed = request.AttemptsAllowed,
            };

            _quizService.CreateQuiz(quiz, request.TopicId);

            return CreatedAtAction(
                nameof(GetQuizById),
                new { quizId = quiz.QuizId },
                CreateSuccessResponse(quiz)
            );
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz to update.</param>
        /// <param name="request">Data representing the updated quiz properties, provided in the request body.</param>
        /// <response code="204">No content on successful update.</response>
        /// <response code="400">Bad request due to invalid input (e.g., missing or empty name, negative duration, etc.).</response>
        /// <response code="404">Not found if no quiz exists with the provided ID.</response>

        [HttpPut("{quizId}")]
        public IActionResult UpdateQuiz(Guid quizId, [FromBody] UpdateQuizViewModel request)
        {
            var existingQuiz = _quizService.GetQuizById(quizId);

            if (existingQuiz == null)
                return NotFound(CreateFailureResponse($"Quiz with id {quizId} not found.", 404));

            existingQuiz.NameOfQuiz = request.NameOfQuiz;
            existingQuiz.Duration = request.Duration;
            existingQuiz.PassMark = request.PassMark;
            existingQuiz.AttemptsAllowed = request.AttemptsAllowed;

            _quizService.UpdateQuiz(existingQuiz);

            return NoContent();
        }

        [HttpGet("topic/{topicId}")]
        public IActionResult GetQuizIdByTopicId(Guid topicId)
        {
            var quizId = _quizService.GetQuizIdByTopicId(topicId);

            if (quizId == null)
                return NotFound(
                    CreateFailureResponse($"No quiz found for topic id {topicId}.", 404)
                );

            return Ok(CreateSuccessResponse(quizId));
        }

        /// <summary>
        /// Deletes a quiz by its ID.
        /// </summary>
        /// <param name="quizId" >The unique identifier of the quiz to delete.</param>
        /// <response code="204">No content on successful deletion.</response>
        /// <response code="404">Not found if no quiz exists with the provided ID.</response>

        [HttpDelete("{quizId}")]
        public IActionResult DeleteQuiz(Guid quizId)
        {
            var existingQuiz = _quizService.GetQuizById(quizId);

            if (existingQuiz == null)
                return NotFound(CreateFailureResponse($"Quiz with id {quizId} not found.", 404));

            _quizService.DeleteQuiz(quizId);

            return NoContent();
        }
    }
}
