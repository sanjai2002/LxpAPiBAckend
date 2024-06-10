using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Mapping;

namespace LXP.Api.Controllers
{

    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class LearnerAttemptController : BaseController
    {
        private readonly ILearnerAttemptServices _services;

        public LearnerAttemptController(ILearnerAttemptServices services)
        {
            _services = services;
        }

        /// <summary>
        ///  Getting score by Topic Id and Learner ID  ---------------Ruban code    
        /// </summary>
        
        [HttpGet]
        public IActionResult GetScoreByTopicIdAndLearnerId(string TopicId,string LearnerId)
        {
            return Ok(CreateSuccessResponse(_services.GetScoreByTopicIdAndLernerId(TopicId,LearnerId)));
        }


        [HttpGet]
        public IActionResult GetScoreByLearnerId(string LearnerId)
        {
            return Ok(CreateSuccessResponse(_services.GetScoreByLearnerId(LearnerId)));

        }

    }
}
