using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizFeedbackController : ControllerBase
    {
        private readonly IQuizFeedbackService _quizFeedbackService;

        public QuizFeedbackController(IQuizFeedbackService quizFeedbackService)
        {
            _quizFeedbackService = quizFeedbackService;
        }

        [HttpPost("AddFeedbackQuestion")]
        public IActionResult AddFeedbackQuestion([FromBody] QuizFeedbackQuestionDto quizfeedbackquestionDto)
        {
            var result = _quizFeedbackService.AddFeedbackQuestion(quizfeedbackquestionDto, quizfeedbackquestionDto.Options);
            return Ok(result); 
        }

        [HttpGet("GetAllFeedbackQuestions")]
        public IActionResult GetAllFeedbackQuestions()
        {
            var questions = _quizFeedbackService.GetAllFeedbackQuestions();
            return Ok(questions);
        }

        [HttpGet("GetFeedbackQuestionById/{quizFeedbackQuestionId}")]
        public IActionResult GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
        {
            var question = _quizFeedbackService.GetFeedbackQuestionById(quizFeedbackQuestionId);
            if (question == null)
                return NotFound(); 
            return Ok(question);
        }

        [HttpPut("UpdateFeedbackQuestion/{quizFeedbackQuestionId}")]
        public IActionResult UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, [FromBody] QuizFeedbackQuestionDto quizfeedbackquestionDto)
        {
            var result = _quizFeedbackService.UpdateFeedbackQuestion(quizFeedbackQuestionId, quizfeedbackquestionDto, quizfeedbackquestionDto.Options);
            if (!result)
               return NotFound(); 
            return NoContent(); 
        }

        [HttpDelete("DeleteFeedbackQuestion/{quizFeedbackQuestionId}")]
        public IActionResult DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
        {
            var result = _quizFeedbackService.DeleteFeedbackQuestion(quizFeedbackQuestionId);
            if (!result)
                return NotFound(); 
            return NoContent(); 
        }
        [HttpGet("GetFeedbackQuestionsByQuizId/{quizId}")]
        public IActionResult GetFeedbackQuestionsByQuizId(Guid quizId)
        {
            var questions = _quizFeedbackService.GetFeedbackQuestionsByQuizId(quizId);
            if (questions == null || !questions.Any())
                return NotFound();
            return Ok(questions);
        }

        [HttpDelete("DeleteFeedbackQuestionsByQuizId/{quizId}")]
        public IActionResult DeleteFeedbackQuestionsByQuizId(Guid quizId)
        {
            var result = _quizFeedbackService.DeleteFeedbackQuestionsByQuizId(quizId);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
