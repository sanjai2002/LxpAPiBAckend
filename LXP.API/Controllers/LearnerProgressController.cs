using LXP.Common.Constants;
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

        [HttpPost("/lxp/learner/learnerprogressStatus")]
        public async Task MaterialCompleted(Guid learnerId,Guid materialId)
        {
            await _Progress.materialCompletion(learnerId,materialId);
        }


    }
}
