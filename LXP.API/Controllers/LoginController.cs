using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ILoginService _services;

        public LoginController(ILoginService services)
        {
            _services = services;
        }


        ///<summary>
        ///Login for Leaners along with their Role (Admin and User)
        ///</summary>


        [HttpPost]

        public async Task<ActionResult> LoginLearner([FromBody] LoginModel loginmodel)
        {

            LoginRole data = await _services.LoginLearner(loginmodel);

            return Ok(data);
        }


        //[HttpPost]
        //public async Task<ActionResult> GetLearnerId([FromBody] EmailViewModel emailViewModel)
        //{

        //    Guid Learnerid = await _services.GetLearnerId(emailViewModel);
        //    return Ok(new { userId = Learnerid });

        //}




    }
}