using LXP.Common.ViewModels.QuizEngineViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizEngineController : ControllerBase
    {
        private readonly IQuizEngineService _quizEngineService;

        public QuizEngineController(IQuizEngineService quizEngineService)
        {
            _quizEngineService = quizEngineService;
        }

        /// <summary>
        /// Retrieves a specific quiz by its ID.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz to retrieve.</param>
        /// <response code="200">Success on finding the quiz. The response body contains a basic representation of the quiz data.</response>
        /// <response code="404">Not found if no quiz exists with the provided ID.</response>
        [HttpGet("quiz/{quizId}")]
        public async Task<IActionResult> GetQuizDetails(Guid quizId)
        {
            var quizDetails = await _quizEngineService.GetQuizByIdAsync(quizId);
            if (quizDetails == null)
            {
                return NotFound($"Quiz with ID {quizId} not found.");
            }
            return Ok(quizDetails);
        }

        /// <summary>
        /// Retrieves quiz details by topic ID.
        /// </summary>
        /// <param name="topicId">The unique identifier of the topic.</param>
        /// <response code="200">Success on finding the quiz. The response body contains the quiz data.</response>
        /// <response code="404">Not found if no quiz exists for the provided topic ID.</response>
        [HttpGet("topic/{topicId}/quiz")]
        public async Task<IActionResult> GetQuizDetailsByTopicId(Guid topicId)
        {
            var quizDetails = await _quizEngineService.GetQuizDetailsByTopicIdAsync(topicId);
            if (quizDetails == null)
            {
                return NotFound($"No quiz found for topic with ID {topicId}.");
            }
            return Ok(quizDetails);
        }

        /// <summary>
        /// Retrieves questions for a specific quiz by its ID.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <response code="200">Success on finding the questions. The response body contains the list of questions for the quiz.</response>
        [HttpGet("quiz/{quizId}/questions")]
        public async Task<IActionResult> GetQuizQuestions(Guid quizId)
        {
            var questions = await _quizEngineService.GetQuestionsForQuizAsync(quizId);
            return Ok(questions);
        }

        /// <summary>
        /// Starts a quiz attempt for a learner.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner.</param>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <response code="200">Success on starting the quiz attempt. The response body contains the attempt ID.</response>
        [HttpPost("attempt")]
        public async Task<IActionResult> StartQuizAttempt(Guid learnerId, Guid quizId)
        {
            var attemptId = await _quizEngineService.StartQuizAttemptAsync(learnerId, quizId);
            return Ok(attemptId);
        }

        /// <summary>
        /// Submits an answer for a quiz question.
        /// </summary>
        /// <param name="answerSubmissionModel">The model containing the answer submission details.</param>
        /// <response code="200">Success on submitting the answer.</response>
        [HttpPost("answer")]
        public async Task<IActionResult> SubmitAnswer(AnswerSubmissionModel answerSubmissionModel)
        {
            await _quizEngineService.SubmitAnswerAsync(answerSubmissionModel);
            return Ok();
        }

        /// <summary>
        /// Retrieves details of a specific quiz attempt by its ID.
        /// </summary>
        /// <param name="attemptId">The unique identifier of the quiz attempt.</param>
        /// <response code="200">Success on finding the quiz attempt. The response body contains the attempt details.</response>
        [HttpGet("attempts/{attemptId}")]
        public async Task<IActionResult> GetLearnerQuizAttempt(Guid attemptId)
        {
            var viewModel = await _quizEngineService.GetLearnerQuizAttemptAsync(attemptId);
            return Ok(viewModel);
        }

        /// <summary>
        /// Submits a quiz attempt by its ID.
        /// </summary>
        /// <param name="attemptId">The unique identifier of the quiz attempt.</param>
        /// <response code="200">Success on submitting the quiz attempt.</response>
        [HttpPost("attempt/submit")]
        public async Task<IActionResult> SubmitQuizAttempt(Guid attemptId)
        {
            await _quizEngineService.SubmitQuizAttemptAsync(attemptId);
            return Ok();
        }

        /// <summary>
        /// Retrieves the result of a specific quiz attempt by its ID.
        /// </summary>
        /// <param name="attemptId">The unique identifier of the quiz attempt.</param>
        /// <response code="200">Success on finding the quiz attempt result. The response body contains the result details.</response>
        [HttpGet("attempts/{attemptId}/result")]
        public async Task<IActionResult> GetLearnerQuizAttemptResult(Guid attemptId)
        {
            var viewModel = await _quizEngineService.GetLearnerQuizAttemptResultAsync(attemptId);
            return Ok(viewModel);
        }

        /// <summary>
        /// Allows a learner to retake a specific quiz.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner.</param>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <response code="200">Success on retaking the quiz. The response body contains the new attempt ID.</response>
        [HttpPost("retake")]
        public async Task<IActionResult> RetakeQuiz(Guid learnerId, Guid quizId)
        {
            var attemptId = await _quizEngineService.RetakeQuizAsync(learnerId, quizId);
            return Ok(attemptId);
        }

        /// <summary>
        /// Retrieves the status of a quiz for a specific learner. for frontend
        /// </summary>
        /// <param name="learnerId">The ID of the learner.</param>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <returns>A response containing the quiz status for the learner.</returns>
        /// <response code="200">Quiz status retrieved successfully.</response>
        /// <response code="404">No data found for the given learnerId and quizId.</response>

        [HttpGet("learner/{learnerId}/quiz/{quizId}/status")]
        public async Task<ActionResult<LearnerQuizStatusViewModel>> GetLearnerQuizStatus(
            Guid learnerId,
            Guid quizId
        )
        {
            var status = await _quizEngineService.GetLearnerQuizStatusAsync(learnerId, quizId);
            return Ok(status);
        }

        /// <summary>
        /// Retrieves the pass status of a specific learner by their attempt ID. for forntend
        /// </summary>
        /// <param name="learnerAttemptId">The unique identifier of the learner's attempt to retrieve.</param>
        /// <response code="200">Success on finding the learner's attempt. The response body contains a boolean representation of the pass status.</response>
        /// <response code="404">Not found if no learner attempt exists with the provided ID.</response>
        [HttpGet("learner/{learnerAttemptId}/passstatus")]
        public async Task<IActionResult> GetPassStatus(Guid learnerAttemptId)
        {
            var result = await _quizEngineService.CheckLearnerPassStatusAsync(learnerAttemptId);
            return Ok(result);
        }

        /// <summary>
        /// Api  Endpoint in development stage not ready to use yet
        /// </summary>

        [HttpPost("SubmitAnswerBatch")]
        public async Task<IActionResult> SubmitAnswerBatch(
            [FromBody] AnswerSubmissionBatchModel model
        )
        {
            try
            {
                await _quizEngineService.SubmitAnswerBatchAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Api  Endpoint in development stage not ready to use yet
        /// </summary>

        [HttpPost("attempt/submit-cached-answers")]
        public async Task<IActionResult> SubmitCachedAnswers(Guid learnerAttemptId)
        {
            try
            {
                await _quizEngineService.SubmitCachedAnswersAsync(learnerAttemptId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Api  Endpoint in development stage not ready to use yet
        /// </summary>
        [HttpPost("attempt/cache-answers")]
        public async Task<IActionResult> CacheAnswers([FromBody] CachedAnswerSubmissionModel model)
        {
            try
            {
                await _quizEngineService.CacheAnswersAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
