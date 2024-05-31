using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LXP.Core.IServices;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseLevelController : BaseController
    {
        private readonly ICourseLevelServices _courseLevelServices;
        public CourseLevelController(ICourseLevelServices courseLevelServices)
        {
            this._courseLevelServices = courseLevelServices;
        }

        [HttpGet("/lxp/course/courselevel/{id}")]
        public async Task<IActionResult> GetAllCourseLevel(string id)
        {
            return Ok(CreateSuccessResponse(await _courseLevelServices.GetAllCourseLevel(id)));
        }
    }
}