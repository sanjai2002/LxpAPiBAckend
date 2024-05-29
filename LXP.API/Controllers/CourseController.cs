using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LXP.Data;
namespace LXP.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    ///<summary>
    ///course list
    ///</summary>
    public class CourseController : BaseController
    {

        private readonly ICourseServices _courseServices;

        public CourseController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        [HttpPost("/lxp/course")]
        public IActionResult AddCourseDetails(CourseViewModel course)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CourseListViewModel CreatedCourse = _courseServices.AddCourse(course);
            if (CreatedCourse != null)
            {
                return Ok(CreateInsertResponse(CreatedCourse));
            }
            return Ok(CreateFailureResponse(MessageConstants.MsgAlreadyExists, (int)HttpStatusCode.PreconditionFailed));


        }
        [HttpGet("/lxp/course/{id}")]
        public async Task<IActionResult> GetCourseDetailsByCourseId(string id)
        {
            CourseListViewModel course = await _courseServices.GetCourseDetailsByCourseId(id);
            return Ok(CreateSuccessResponse(course));
        }

        /////<summary>
        /////Getting all Course name and its id
        /////</summary>
        /////<response code="200">Success</response>
        /////<response code="404">Internal server Error</response>
        //[HttpGet("get/course/{courseId}")]
        //public ActionResult<Course> GetById(Guid courseId) 
        //{
        //    var course = _courseServices.GetCourseByCourseId(courseId);
        //    if(course == null)
        //    {
        //        return Ok(CreateFailureResponse(MessageConstants.MsgGetbyid,(int)HttpStatusCode.NotFound));
        //    }
        //    return Ok(CreateSuccessResponse(course));
        //}

        ///<summary>
        ///Update the course
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="405">Internal server Error</response>
        [HttpPut("lxp/courseupdate")]
        public async Task<IActionResult> Updatecourse([FromForm]CourseUpdateModel course)
        {
            bool updatecourse = await _courseServices.Updatecourse(course);

            if (updatecourse == true)
            {
                return Ok(CreateSuccessResponse(updatecourse));
            }

            return Ok(CreateFailureResponse(MessageConstants.MsgNotUpdated,(int)HttpStatusCode.MethodNotAllowed));
        }


        ///<summary>
        ///Delete the course
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="405">Internal server Error</response>
        [HttpDelete("/lxp/coursedelete/{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            bool course = await _courseServices.Deletecourse(id);

            if (course == true)
            {
                return Ok(CreateSuccessResponse(course));
            }
            return Ok(CreateFailureResponse(MessageConstants.MsgCourseNotDeleted, (int)HttpStatusCode.MethodNotAllowed));
        }


        ///<summary>
        ///Update the course status 
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="405">Internal server Error</response>
        
        [HttpPut("/lxp/coursestatus")]
        public async Task<IActionResult> Coursestatus(Coursestatus CourseStatus)
        {
            bool Coursestatus =await _courseServices.Changecoursestatus(CourseStatus);

            if (Coursestatus == true)
            {
                return Ok(CreateSuccessResponse(Coursestatus));
            }
            return Ok(CreateFailureResponse(MessageConstants.MsgNotUpdated,(int)HttpStatusCode.MethodNotAllowed));

        }

        [HttpGet("lxp/GetAllCourse")]
        public IActionResult GetAllCourse()
        {
            var courses = _courseServices.GetAllCourse();
            return Ok(CreateSuccessResponse(courses));
        }


        [HttpGet("lxp/Getninecourse")]
        public IActionResult GetLimitedCourse()
        {
            var course = _courseServices.GetLimitedCourse();
            return Ok(CreateSuccessResponse(course));
        }


}
}
