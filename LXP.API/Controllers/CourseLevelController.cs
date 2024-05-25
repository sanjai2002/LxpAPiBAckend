using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    ///<summary>
    /// course Level
    ///</summary>
      public class CourseLevelController : BaseController
      {
        private readonly ICourseLevelServices _courseLevelServices;
        public CourseLevelController(ICourseLevelServices courseLevelServices)
        {
            _courseLevelServices = courseLevelServices;
        }


        ///<summary>
        ///Getting all Course level and  id
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="404">Internal server Error</response>
        ///
        //[HttpGet("/lxp/course/courselevel/{id}")]
        //public async Task<IActionResult> GetAllCourseLevel(string id)
        //{
        //    return Ok(CreateSuccessResponse(await _courseLevelServices.GetAllCourseLevel(id)));
        //}
        [HttpGet("/lxp/course/courselevel/{accessedBy}")]
        public async Task<IActionResult> GetAllCourseLevel(string accessedBy)
        {
            return Ok(CreateSuccessResponse(await _courseLevelServices.GetAllCourseLevel(accessedBy)));
        }
    }
}
