﻿using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    ///<summary>
    ///Handles operations related to quiz questions.
    ///</summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class QuizQuestionsController : BaseController
    {
        private readonly IQuizQuestionService _quizQuestionService;

        public QuizQuestionsController(IQuizQuestionService quizQuestionService)
        {
            _quizQuestionService = quizQuestionService;
        }

        ///<summary>
        ///Add a new quiz question.
        ///</summary>
        ///<param name="quizQuestion">The details of the quiz question to add.</param>
        ///<response code="200">Quiz question added successfully.</response>
        ///<response code="500">Internal server error.</response>

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] QuizQuestionViewModel quizQuestion)
        {
            var result = await _quizQuestionService.AddQuestionAsync(
                quizQuestion,
                quizQuestion.Options
            );
            return Ok(CreateSuccessResponse(result));
        }

        ///<summary>
        ///Update an existing quiz question.
        ///</summary>
        ///<param name="quizQuestionId">The ID of the quiz question to update.</param>
        ///<param name="quizQuestion">The updated details of the quiz question.</param>
        ///<response code="200">Quiz question updated successfully.</response>
        ///<response code="404">Quiz question not found.</response>
        ///<response code="500">Internal server error.</response>

        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestion(
            Guid quizQuestionId,
            [FromBody] QuizQuestionViewModel quizQuestion
        )
        {
            var existingQuestion = await _quizQuestionService.GetQuestionByIdAsync(quizQuestionId);
            if (existingQuestion == null)
                return NotFound(
                    CreateFailureResponse($"Quiz question with ID {quizQuestionId} not found.", 404)
                );

            var result = await _quizQuestionService.UpdateQuestionAsync(
                quizQuestionId,
                quizQuestion,
                quizQuestion.Options
            );
            return Ok(CreateSuccessResponse(result));
        }

        ///<summary>
        ///Delete a quiz question.
        ///</summary>
        ///<param name="quizQuestionId">The ID of the quiz question to delete.</param>
        ///<response code="200">Quiz question deleted successfully.</response>
        ///<response code="404">Quiz question not found.</response>
        ///<response code="500">Internal server error.</response>
        [HttpDelete("DeleteQuestion")]
        public async Task<IActionResult> DeleteQuestion(Guid quizQuestionId)
        {
            var existingQuestion = await _quizQuestionService.GetQuestionByIdAsync(quizQuestionId);
            if (existingQuestion == null)
                return NotFound(
                    CreateFailureResponse($"Quiz question with ID {quizQuestionId} not found.", 404)
                );

            var result = await _quizQuestionService.DeleteQuestionAsync(quizQuestionId);
            return Ok(CreateSuccessResponse(result));
        }

        ///<summary>
        ///Retrieve all quiz questions.
        ///</summary>
        ///<response code="200">List of all quiz questions.</response>
        ///<response code="500">Internal server error.</response>
        //[HttpGet("GetAllQuestions")]
        //public IActionResult GetAllQuestions()
        //{
        //    var result = _quizQuestionService.GetAllQuestionsAsync();
        //    return Ok(CreateSuccessResponse(result));
        //}
        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var result = await _quizQuestionService.GetAllQuestionsAsync();
            return Ok(CreateSuccessResponse(result));
        }

        ///<summary>
        ///Retrieve all quiz questions for a specific quiz.
        ///</summary>
        ///<param name="quizId">The ID of the quiz.</param>
        ///<response code="200">List of quiz questions for the specified quiz.</response>
        ///<response code="404">Quiz questions not found.</response>
        ///<response code="500">Internal server error.</response>

        [HttpGet("GetAllQuestionsByQuizId")]
        public async Task<IActionResult> GetAllQuestionsByQuizId(Guid quizId)
        {
            var result = await _quizQuestionService.GetAllQuestionsByQuizIdAsync(quizId);
            if (result == null || !result.Any())
                return NotFound(
                    CreateFailureResponse($"No quiz questions found for quiz ID {quizId}.", 404)
                );
            return Ok(CreateSuccessResponse(result));
        }

        ///<summary>
        ///Retrieve a quiz question by its ID.
        ///</summary>
        ///<param name="quizQuestionId">The ID of the quiz question.</param>
        ///<response code="200">Quiz question details.</response>
        ///<response code="404">Quiz question not found.</response>
        ///<response code="500">Internal server error.</response>
        [HttpGet("GetQuestionById")]
        public async Task<IActionResult> GetQuestionById(Guid quizQuestionId)
        {
            var result = await _quizQuestionService.GetQuestionByIdAsync(quizQuestionId);
            if (result == null)
                return NotFound(
                    CreateFailureResponse($"Quiz question with ID {quizQuestionId} not found.", 404)
                );
            return Ok(CreateSuccessResponse(result));
        }
    }
}
