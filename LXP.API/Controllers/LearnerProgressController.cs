using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILearnerProgressService _Progress;
        public LearnerProgressController(ILearnerProgressService Progress)
        {
            _Progress = Progress;
        }

        [HttpPost("/lxp/course/learnerprogress")]

        public async Task<IActionResult> MaterialProgress(LearnerProgressViewModel learnerProgress)
        {
         await _Progress.Progress(learnerProgress);
            return Ok();
            
           
        }
        
        //[HttpPost("/lxp/learner/learnerprogressStatus")]
        //public async Task MaterialCompleted(Guid learnerId,Guid courseId)
        //{
        //    await _Progress.materialCompletion(learnerId,courseId);
        //}
        [HttpPost("/lxp/learner/learnerprogressWatchTime")]
        public async Task<IActionResult> MaterialWatchTime(Guid learnerId,Guid materialId,TimeOnly watchtime)
        {
           double percentage= await _Progress.materialWatchTime(learnerId, materialId, watchtime);
            return Ok(percentage);
        }




        //[HttpPost("/lxp/learner/learnerprogressPercentage")]
        //public async Task<IActionResult> MaterialPercentage(Guid learnerId, Guid courseId)
        //{

        //   double percentage= await _Progress.materialCompletionPercentage(learnerId, courseId);
        //    return Ok(percentage);
        //}



    }
}
