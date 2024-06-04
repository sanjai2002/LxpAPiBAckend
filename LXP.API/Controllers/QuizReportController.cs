using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizReportController : BaseController
    {
        private readonly IQuizReportServices _quizReportServices;
        public QuizReportController(IQuizReportServices quizReportServices)
        {
            _quizReportServices = quizReportServices;

        }

        /// <summary>
        /// Report for Quiz
        /// </summary>
        [HttpGet("QuizReport")]

        public IActionResult GetQuizReport()
        {
            var report = _quizReportServices.GetQuizReports();
            return Ok(CreateSuccessResponse(report));
        }

        /// <summary>
        /// GetPassdLearnersList
        /// </summary>
        [HttpGet("QuizReport/passedlearnersReport/{Quizid}!")]

        public IActionResult GetPassdLearnersList(Guid Quizid)
        {
            var PassesLearners = _quizReportServices.GetPassdLearnersList(Quizid);
            return Ok(CreateSuccessResponse(PassesLearners));
        }

        /// <summary>
        /// GetFailedLearnersList
        /// </summary>
        [HttpGet("QuizReport/FailedlearnersReport/{Quizid}")]

        public IActionResult GetFailedLearnersList(Guid Quizid)
        {
            var PassesLearners = _quizReportServices.GetFailedLearnersList(Quizid);
            return Ok(CreateSuccessResponse(PassesLearners));

        }


    }
}
