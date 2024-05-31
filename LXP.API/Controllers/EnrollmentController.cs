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


    }
}