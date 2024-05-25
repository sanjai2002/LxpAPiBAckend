using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LXP.Api.Controllers
{
    /// <summary>
    /// Manages bulk question import operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BulkQuestionController : BaseController // Inherit from BaseController
    {
        private readonly IBulkQuestionService _excelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkQuestionController"/> class.
        /// </summary>
        /// <param name="excelService">The bulk question service.</param>
        public BulkQuestionController(IBulkQuestionService excelService)
        {
            _excelService = excelService;
        }

        /// <summary>
        /// Imports quiz data from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing quiz data.</param>
        /// <param name="quizId">The unique identifier of the quiz to which the data belongs.</param>
        /// <returns>A response indicating the result of the import operation.</returns>
        /// <response code="200">Quiz data imported successfully.</response>
        /// <response code="400">Bad request due to invalid input.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("ImportQuizData")]
        public async Task<IActionResult> ImportQuizData(IFormFile file, Guid quizId)
        {
            // Validate the model
            var validationResponse = ValidateImportQuizData(file, quizId);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            try
            {
                var result = await _excelService.ImportQuizDataAsync(file, quizId);
                return Ok(CreateSuccessResponse(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateFailureResponse($"An error occurred while importing quiz data: {ex.Message}", 500));
            }
        }

        /// <summary>
        /// Validates the import quiz data request.
        /// </summary>
        /// <param name="file">The Excel file containing quiz data.</param>
        /// <param name="quizId">The unique identifier of the quiz to which the data belongs.</param>
        /// <returns>A validation response if validation fails, otherwise null.</returns>
        private IActionResult ValidateImportQuizData(IFormFile file, Guid quizId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(CreateFailureResponse("The file is required and cannot be empty.", 400));
            }

            if (quizId == Guid.Empty)
            {
                return BadRequest(CreateFailureResponse("The quiz ID must be a valid GUID.", 400));
            }

            return null;
        }
    }
}


//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks; 

//namespace LXP.Api.controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BulkQuestionController : ControllerBase
//    {
//        private readonly IBulkQuestionService _excelService;

//        public BulkQuestionController(IBulkQuestionService excelService)
//        {
//            _excelService = excelService;
//        }

//        [HttpPost("ImportQuizData")]
//        public async Task<IActionResult> ImportQuizData(IFormFile file, Guid quizId) // Change the parameter type to Guid
//        {
//            try
//            {
//                var result = await _excelService.ImportQuizDataAsync(file, quizId);

//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"An error occurred while importing quiz data: {ex.Message}");
//            }
//        }
//    }
//}




//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using OfficeOpenXml;

//namespace LXP.Api.controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BulkQuestionController : ControllerBase
//    {
//        private readonly IBulkQuestionService _excelService;

//        public BulkQuestionController(IBulkQuestionService excelService)
//        {
//            _excelService = excelService;
//        }

//        [HttpPost("ImportQuizData")]
//        public async Task<IActionResult> ImportQuizData(IFormFile file, string quizName)
//        {
//            try
//            {
//                var result = await _excelService.ImportQuizDataAsync(file, quizName);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"An error occurred while importing quiz data: {ex.Message}");
//            }
//        }
//    }
//}