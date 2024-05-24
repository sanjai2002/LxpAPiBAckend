using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomPasswordController : ControllerBase
    {


        private readonly IService _services;

        public RandomPasswordController(IService services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword([FromBody] RandomPasswordEmail randompassword)

        {
            var randomstore = _services.ForgetPassword(randompassword.Email);

            return Ok(randomstore);

        }


    }
}
