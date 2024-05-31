
using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;


namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    ///<summary>
    ///Handles quiz feedback related operations
    ///</summary>
    public class QuizFeedbackController : BaseController // Inherit from BaseController
    {
        private readonly IQuizFeedbackService _quizFeedbackService;

        public QuizFeedbackController(IQuizFeedbackService quizFeedbackService)
        {
            _quizFeedbackService = quizFeedbackService;
        }

        ///<summary>
        ///Add a new feedback question
        ///</summary>
        ///<param name="quizfeedbackquestionDto">Feedback question details</param>
        ///<response code="200">Feedback question added successfully</response>
        ///<response code="500">Internal server error</response>
        [HttpPost("AddFeedbackQuestion")]
        public IActionResult AddFeedbackQuestion([FromBody] QuizfeedbackquestionViewModel quizfeedbackquestionDto)
        {
            var result = _quizFeedbackService.AddFeedbackQuestion(quizfeedbackquestionDto, quizfeedbackquestionDto.Options);
            return Ok(CreateSuccessResponse(result));
        }

        ///<summary>
        ///Retrieve all feedback questions
        ///</summary>
        ///<response code="200">List of feedback questions</response>
        ///<response code="500">Internal server error</response>
        [HttpGet("GetAllFeedbackQuestions")]
        public IActionResult GetAllFeedbackQuestions()
        {
            var questions = _quizFeedbackService.GetAllFeedbackQuestions();
            return Ok(CreateSuccessResponse(questions));
        }


        ///<summary>
        ///Get a feedback question by its ID
        ///</summary>
        ///<param name="quizFeedbackQuestionId">Feedback question ID</param>
        ///<response code="200">Feedback question details</response>
        ///<response code="404">Feedback question not found</response>
        ///<response code="500">Internal server error</response>
        [HttpGet("GetFeedbackQuestionById/{quizFeedbackQuestionId}")]
        public IActionResult GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
        {
            var question = _quizFeedbackService.GetFeedbackQuestionById(quizFeedbackQuestionId);
            if (question == null)
                return NotFound(CreateFailureResponse($"Feedback question with ID {quizFeedbackQuestionId} not found.", 404));
            return Ok(CreateSuccessResponse(question));
        }

        ///<summary>
        ///Update a feedback question
        ///</summary>
        ///<param name="quizFeedbackQuestionId">Feedback question ID</param>
        ///<param name="quizfeedbackquestionDto">Updated feedback question details</param>
        ///<response code="204">Feedback question updated successfully</response>
        ///<response code="404">Feedback question not found</response>
        ///<response code="500">Internal server error</response>
        [HttpPut("UpdateFeedbackQuestion/{quizFeedbackQuestionId}")]
        public IActionResult UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, [FromBody] QuizfeedbackquestionViewModel quizfeedbackquestionDto)
        {
            var existingQuestion = _quizFeedbackService.GetFeedbackQuestionById(quizFeedbackQuestionId);
            if (existingQuestion == null)
                return NotFound(CreateFailureResponse($"Feedback question with ID {quizFeedbackQuestionId} not found.", 404));

            var result = _quizFeedbackService.UpdateFeedbackQuestion(quizFeedbackQuestionId, quizfeedbackquestionDto, quizfeedbackquestionDto.Options);
            if (!result)
                return NotFound(CreateFailureResponse($"Failed to update feedback question with ID {quizFeedbackQuestionId}.", 500));

            return NoContent();
        }

        ///<summary>
        ///Delete a feedback question
        ///</summary>
        ///<param name="quizFeedbackQuestionId">Feedback question ID</param>
        ///<response code="204">Feedback question deleted successfully</response>
        ///<response code="404">Feedback question not found</response>
        ///<response code="500">Internal server error</response>
        [HttpDelete("DeleteFeedbackQuestion/{quizFeedbackQuestionId}")]
        public IActionResult DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
        {
            var existingQuestion = _quizFeedbackService.GetFeedbackQuestionById(quizFeedbackQuestionId);
            if (existingQuestion == null)
                return NotFound(CreateFailureResponse($"Feedback question with ID {quizFeedbackQuestionId} not found.", 404));

            var result = _quizFeedbackService.DeleteFeedbackQuestion(quizFeedbackQuestionId);
            if (!result)
                return NotFound(CreateFailureResponse($"Failed to delete feedback question with ID {quizFeedbackQuestionId}.", 500));

            return NoContent();
        }

        ///<summary>
        ///Get feedback questions by quiz ID
        ///</summary>
        ///<param name="quizId">Quiz ID</param>
        ///<response code="200">List of feedback questions</response>
        ///<response code="404">Feedback questions not found</response>
        ///<response code="500">Internal server error</response>
        [HttpGet("GetFeedbackQuestionsByQuizId/{quizId}")]
        public IActionResult GetFeedbackQuestionsByQuizId(Guid quizId)
        {
            var questions = _quizFeedbackService.GetFeedbackQuestionsByQuizId(quizId);
            if (questions == null || !questions.Any())
                return NotFound(CreateFailureResponse($"No feedback questions found for quiz ID {quizId}.", 404));
            return Ok(CreateSuccessResponse(questions));
        }


     

        ///<summary>
        ///Delete feedback questions by quiz ID
        ///</summary>
        ///<param name="quizId">Quiz ID</param>
        ///<response code="204">Feedback questions deleted successfully</response>
        ///<response code="404">Feedback questions not found</response>
        ///<response code="500">Internal server error</response>
        [HttpDelete("DeleteFeedbackQuestionsByQuizId/{quizId}")]
        public IActionResult DeleteFeedbackQuestionsByQuizId(Guid quizId)
        {
            var questions = _quizFeedbackService.GetFeedbackQuestionsByQuizId(quizId);
            if (questions == null || !questions.Any())
                return NotFound(CreateFailureResponse($"No feedback questions found for quiz ID {quizId}.", 404));

            var result = _quizFeedbackService.DeleteFeedbackQuestionsByQuizId(quizId);
            if (!result)
                return NotFound(CreateFailureResponse($"Failed to delete feedback questions for quiz ID {quizId}.", 500));

            return NoContent();
        }
    }
}

//using LXP.Common.DTO;
//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//namespace LXP.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QuizFeedbackController : ControllerBase
//    {
//        private readonly IQuizFeedbackService _quizFeedbackService;

//        public QuizFeedbackController(IQuizFeedbackService quizFeedbackService)
//        {
//            _quizFeedbackService = quizFeedbackService;
//        }

//        [HttpPost("AddFeedbackQuestion")]
//        public IActionResult AddFeedbackQuestion([FromBody] QuizFeedbackQuestionDto quizfeedbackquestionDto)
//        {
//            var result = _quizFeedbackService.AddFeedbackQuestion(quizfeedbackquestionDto, quizfeedbackquestionDto.Options);
//            return Ok(result); 
//        }

//        [HttpGet("GetAllFeedbackQuestions")]
//        public IActionResult GetAllFeedbackQuestions()
//        {
//            var questions = _quizFeedbackService.GetAllFeedbackQuestions();
//            return Ok(questions);
//        }

//        [HttpGet("GetFeedbackQuestionById/{quizFeedbackQuestionId}")]
//        public IActionResult GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
//        {
//            var question = _quizFeedbackService.GetFeedbackQuestionById(quizFeedbackQuestionId);
//            if (question == null)
//                return NotFound(); 
//            return Ok(question);
//        }

//        [HttpPut("UpdateFeedbackQuestion/{quizFeedbackQuestionId}")]
//        public IActionResult UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, [FromBody] QuizFeedbackQuestionDto quizfeedbackquestionDto)
//        {
//            var result = _quizFeedbackService.UpdateFeedbackQuestion(quizFeedbackQuestionId, quizfeedbackquestionDto, quizfeedbackquestionDto.Options);
//            if (!result)
//               return NotFound(); 
//            return NoContent(); 
//        }

//        [HttpDelete("DeleteFeedbackQuestion/{quizFeedbackQuestionId}")]
//        public IActionResult DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
//        {
//            var result = _quizFeedbackService.DeleteFeedbackQuestion(quizFeedbackQuestionId);
//            if (!result)
//                return NotFound(); 
//            return NoContent(); 
//        }
//        [HttpGet("GetFeedbackQuestionsByQuizId/{quizId}")]
//        public IActionResult GetFeedbackQuestionsByQuizId(Guid quizId)
//        {
//            var questions = _quizFeedbackService.GetFeedbackQuestionsByQuizId(quizId);
//            if (questions == null || !questions.Any())
//                return NotFound();
//            return Ok(questions);
//        }

//        [HttpDelete("DeleteFeedbackQuestionsByQuizId/{quizId}")]
//        public IActionResult DeleteFeedbackQuestionsByQuizId(Guid quizId)
//        {
//            var result = _quizFeedbackService.DeleteFeedbackQuestionsByQuizId(quizId);
//            if (!result)
//                return NotFound();
//            return NoContent();
//        }
//    }
//}


