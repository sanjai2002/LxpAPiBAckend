using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using LXP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class LearnerController : BaseController
    {
        private readonly ILearnerServices _learnerServices;

        public LearnerController(ILearnerServices learnerServices)
        {
            _learnerServices = learnerServices;
        }


        ///<summary>
        ///Learner profile by learner id 
        ///</summary>
        ///<response code="200">Success</response>
        [HttpGet("/lxp/learner/{learnerid}/learnerdetails")]
        public IActionResult GetAllLearnerDetailsByLearnerId(Guid learnerid )
        {
            var learner = _learnerServices.GetAllLearnerDetailsByLearnerId(learnerid);
            return Ok(CreateSuccessResponse(learner));
        }
        ///<summary>
        ///Enrolled course details by learner id
        ///</summary>
        ///<response code="200">Success</response>
 
        [HttpGet("/lxp/learner/{learnerid}/entrolledcourse")]
        public IActionResult GetLearnerEntrolledcourseByLearnerId(Guid learnerid)
        {
            var learner = _learnerServices.GetLearnerEnrolledcourseByLearnerId(learnerid);
            return Ok(CreateSuccessResponse(learner));
        }


  }
}
