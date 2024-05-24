
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks; // Add this using statement

namespace LXP.Api.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkQuestionController : ControllerBase
    {
        private readonly IBulkQuestionService _excelService;

        public BulkQuestionController(IBulkQuestionService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("ImportQuizData")]
        public async Task<IActionResult> ImportQuizData(IFormFile file, Guid quizId) // Change the parameter type to Guid
        {
            try
            {
                var result = await _excelService.ImportQuizDataAsync(file, quizId);
     
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while importing quiz data: {ex.Message}");
            }
        }
    }
}




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
