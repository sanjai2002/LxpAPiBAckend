using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerDashboardController : BaseController
    {
        private readonly ILearnerDashboardService _learnerDashboardService;

        public LearnerDashboardController(ILearnerDashboardService learnerDashboardService)
        {
            _learnerDashboardService = learnerDashboardService;
        }

        [HttpGet("/lxp/learner/LearnerDashboard/{learnerId}")]
        public IActionResult GetLearnerDashboard(Guid learnerId)
        {
            var dashboard = _learnerDashboardService.GetLearnerDashboardDetails(learnerId);

            return Ok(CreateSuccessResponse(dashboard));
        }


    }
}
