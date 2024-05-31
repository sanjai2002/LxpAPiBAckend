using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using LXP.Common.Entities;
using Microsoft.AspNetCore.Mvc;


namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController
    {
        private readonly ICourseServices _courseServices;
        public CourseController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        [HttpPost("/lxp/course")]
        public async Task<IActionResult> AddCourse(CourseViewModel course)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCourseCreated = _courseServices.AddCourse(course);
            if (isCourseCreated)
            {
                return Ok(CreateSuccessResponse(null));
            }
            return Ok(CreateFailureResponse("Not Created", 400));


        }
        [HttpGet("/lxp/course/{id}")]
        public async Task<IActionResult> GetCourseDetails(string id)
        {
            Course course = _courseServices.GetCourseByCourseId(id);
            return Ok(CreateSuccessResponse(course));
        }
        ///<summary>
        ///Fetch all the course
        ///</summary>

        [HttpGet("/lxp/view/course")]

        public IActionResult GetAllCourseDetails()
        {
            var course = _courseServices.GetAllCourseDetails();
            return Ok(CreateSuccessResponse(course));

        }


    }
}