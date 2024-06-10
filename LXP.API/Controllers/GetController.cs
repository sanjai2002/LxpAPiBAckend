using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IQuizFeedbackService _quizFeedbackService;

        public GetController(IQuizService quizService, IQuizFeedbackService quizFeedbackService)
        {
            _quizService = quizService;
            _quizFeedbackService = quizFeedbackService;
        }

        // This controller is used by frontend to get quizrelateddetails


        [HttpGet("topic/{topicId}")]
        public ActionResult<Guid?> GetQuizIdByTopicId(Guid topicId)
        {
            var quizId = _quizService.GetQuizIdByTopicId(topicId);

            if (quizId == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(quizId);
            }
        }
    }
}























//using LXP.Core.IServices;

//using Microsoft.AspNetCore.Mvc;

//namespace LXP.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GetController : ControllerBase
//    {
//        private readonly IQuizService _quizService;
//        private readonly IQuizFeedbackService _quizFeedbackService;

//        public GetController(IQuizService quizService, IQuizFeedbackService quizFeedbackService)
//        {
//            _quizService = quizService;
//            _quizFeedbackService = quizFeedbackService;
//        }
//        // This controller is used by frontend to get quizrelateddetails


//        [HttpGet("topic/{topicId}")]
//        public ActionResult<Guid?> GetQuizIdByTopicId(Guid topicId)

//        {

//            var quizId = _quizService.GetQuizIdByTopicId(topicId);

//            if (quizId == null)

//            {

//                return Ok(null);

//            }

//            else

//            {

//                return Ok(quizId);

//            }

//        }

//    }
//}
