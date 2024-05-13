using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    ///<summary>
    /// Update the course
    ///</summary>
    public class CourseController : BaseController
    {

        private readonly ICourseServices _courseServices;

        public CourseController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        [HttpGet("get/course/{courseId}")]
        public ActionResult<Course> GetById(Guid courseId) 
        {
            var course = _courseServices.GetCourseByCourseId(courseId);
            if(course == null)
            {
                return Ok(CreateFailureResponse("Not available", 404));
            }
            return Ok(CreateSuccessResponse(course));
        }

        //[HttpPut("/lxp/courseupdate/{guid}")]
        //////public ActionResult<Course> UpdateCourse([FromBody] CourseUpdateModel CourseUpdate)
        //public async Task<IActionResult> UpdateCourse(Guid CourseId, CourseUpdateModel CourseUpdate)
        //{
        //    var isCourseUpdated = _courseServices.UpdateCourse(CourseUpdate);
        //    if (isCourseUpdated)
        //    {
        //        return Ok(CreateSuccessResponse(null!));
        //    }
        //    return Ok(CreateFailureResponse("Not Updated", 422));
        //}

        //[HttpPut("lxp/courseupdate")]
        //public async Task<IActionResult> Updatecourse(CourseUpdateModel course)
        //{
        //    var existingcourse = _courseServices.GetCourseByCourseId(course.CourseId);

        //    if (existingcourse == null)
        //    {
        //        return Ok(CreateFailureResponse("not available",422));
        //    }
        //    _courseServices.UpdateCourseusingid(course);

        //    return Ok(CreateSuccessResponse("updated"));
        //}

        [HttpPut("lxp/courseupdate")]
        public async Task<IActionResult> Updatecourse(CourseUpdateModel course)
        {
            bool updatecourse = await _courseServices.Updatecourse(course);

            if (updatecourse == true)
            {
                return Ok(CreateSuccessResponse(updatecourse));
            }

            return Ok(CreateFailureResponse("Not updated",422));
        }





        [HttpDelete("/lxp/coursedelete/{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = _courseServices.Deletecourse(id);

            if (course == true)
            {
                return Ok(CreateSuccessResponse(course));
            }
            return Ok(CreateFailureResponse("Course didn't allow to delete",422));
        }


        [HttpPost("/lxp/coursestatus")]
        public async Task<IActionResult> Coursestatus(Coursestatus CourseStatus)
        {
            bool Coursestatus =await _courseServices.Changecoursestatus(CourseStatus);

            if (Coursestatus == true)
            {
                return Ok(CreateSuccessResponse(Coursestatus));
            }
            return Ok(CreateFailureResponse("status not update", 422));

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
