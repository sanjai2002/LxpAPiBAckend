using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Collections.Concurrent;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http.HttpResults;


namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EnrollmentController : BaseController
    {
        private readonly IEnrollmentService _enrollmentService;


        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("/lxp/enroll")]

        public async Task<IActionResult> Addenroll(EnrollmentViewModel enroll)
        {
            //validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isEnrolled = await _enrollmentService.Addenroll(enroll);

            if (isEnrolled)
            {
                return Ok(CreateSuccessResponse(null));
            }
            return Ok(CreateFailureResponse("AlreadyEnrolled", 400));
        }

        [HttpGet("/lxp/enroll/{learnerId}/course/topic")]

        public IActionResult GetCourseandTopicsByLearnerId(Guid learnerId)
        {
            var learner = _enrollmentService.GetCourseandTopicsByLearnerId(learnerId);
            return Ok(CreateSuccessResponse(learner));
        }

        [HttpGet("lxp/enrollment/report")]

        public IActionResult GetAllEnrollemet()
        {
            var report = _enrollmentService.GetEnrollmentsReport();
            return Ok(CreateSuccessResponse(report));
        }

        [HttpGet("lxp/enrollment/Inprogress/LearnerList")]

        public IActionResult GetInProgressLearnerList(Guid courseId)
        {
            var users = _enrollmentService.GetEnrolledInprogressLearnerbyCourseId(courseId);
            return Ok(CreateSuccessResponse(users));
        }

        [HttpGet("lxp/enrollment/Completed/LearnerList")]
        public IActionResult GetCompletedLearnerList(Guid courseId)
        {
            var users = _enrollmentService.GetEnrolledCompletedLearnerbyCourseId(courseId);
            return Ok(CreateSuccessResponse(users));
        }

    }
}