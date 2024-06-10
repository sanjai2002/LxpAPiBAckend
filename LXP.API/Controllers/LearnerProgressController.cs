using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    ///<summary>
    ///Learner Video progress
    ///</summary>
    public class LearnerProgressController : BaseController
    {
        //private readonly ILearnerProgressService _learnerProgressService;
       private readonly ILearnerProgressService _Progress;
        public LearnerProgressController( ILearnerProgressService Progress)
        {

            _Progress = Progress;
        }

        //[HttpPost("/lxp/course/learnerprogress")]

        //public async Task<IActionResult> MaterialProgress(LearnerProgressViewModel learnerProgress)
        //{
        //    await _Progress.Progress(learnerProgress);
        //    return Ok();


        //}


        [HttpPost("/lxp/course/learner/learnerprogress")]

        public async Task<IActionResult> MaterialProgress(ProgressViewModel learnerProgress)
        {
            var result = await _Progress.LearnerProgress(learnerProgress);
            return Ok(result);
        }



        //[HttpPost("/lxp/learner/learnerprogressStatus")]
        //public async Task MaterialCompleted(Guid learnerId, Guid courseId)
        //{
        //    await _Progress.CalculateMaterialCompletionPercentageAsync(learnerId, courseId);
        //}
        [HttpPost("/lxp/learner/learnerprogressWatchTime")]
        public async Task<IActionResult> MaterialWatchTime(Guid learnerId, Guid materialId, TimeOnly watchtime)
        {
            double percentage = await _Progress.materialWatchTime(learnerId, materialId, watchtime);
            return Ok(percentage);
        }

        [HttpPost("calculate-course-completion/{learnerId}")]
        public async Task<IActionResult> CalculateCourseCompletion(Guid learnerId)
        {
            try
            {
                await _Progress.CalculateAndUpdateCourseCompletionAsync(learnerId);
                return Ok(new { Message = "Course completion percentage calculated and updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while calculating the course completion.", Details = ex.Message });
            }
        }


        [HttpGet("course-completion-percentage/{learnerId}/{enrollmentId}")]
        public async Task<IActionResult> GetCourseCompletionPercentage(Guid learnerId, Guid enrollmentId)
        {
            try
            {
                var percentage = await _Progress.GetCourseCompletionPercentageAsync(learnerId, enrollmentId);
                if (percentage.HasValue)
                {
                    return Ok(new { CourseCompletionPercentage = percentage.Value });
                }
                else
                {
                    return NotFound(new { Message = "Enrollment not found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the course completion percentage.", Details = ex.Message });
            }
        }





    }
}