using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    ///<summary>
    /// Details required for Dashboard
    ///</summary>
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        ///<summary>
        ///getting total number of learners
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="500">Internal server Error</response>
        [HttpGet]
        public IActionResult GetTotalLearners()
        {
            var total_learners = _dashboardService.GetDashboardLearnerList();
            return Ok(CreateSuccessResponse(total_learners.Count()));
        }
        ///<summary>
        ///getting total number of courses
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="500">Internal server Error</response>
        [HttpGet]
        public ActionResult GetTotalCourses()
        {
            var total_courses = _dashboardService.GetDashboardCoursesList();
            return Ok(CreateSuccessResponse(total_courses.Count()));
        }

        ///<summary>
        ///getting total number of enrollments
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="500">Internal server Error</response>
        [HttpGet]
        public ActionResult GetTotalEnrollments()
        {
            var total_enrollments = _dashboardService.GetDashboardEnrollmentList();
            return Ok(CreateSuccessResponse(total_enrollments.Count()));
        }

        ///<summary>
        ///getting total number of enrollments according to month
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="500">Internal server Error</response>
        [HttpGet]
        public ActionResult GetEnrollmentMonth()
        {
            var total_month_wise = _dashboardService.GetMonthEnrollmentList();
            return Ok(CreateSuccessResponse(total_month_wise));
        }

        ///<summary>
        ///getting total number of course creation according to year
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="500">Internal server Error</response>
        [HttpGet]
        public ActionResult GetCourseCreated()
        {
            var total_course_created = _dashboardService.GetCourseCreatedList();
            return Ok(CreateSuccessResponse(total_course_created));
        }

        ///<summary>
        ///getting total number of course creation according to year
        ///</summary>
        ///<response code="200">Success</response>
        ///<response code="500">Internal server Error</response>
        //[HttpGet]
        //public ActionResult GetMostEnrolledCourse()
        //{
        //    var total_course_created = _dashboardService.GetMostEnrolledCourse();
        //    return Ok(CreateSuccessResponse(total_course_created));
        //}



        [HttpGet("/lxp/admin/DashboardDetails")]
        public IActionResult AdminDashboard()
        {
            var admin = _dashboardService.GetAdminDashboardDetails();
            return Ok(CreateSuccessResponse(admin));
        }


    }
}
