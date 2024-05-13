using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {


        private readonly IForgetService _services;

        public ForgetPasswordController(IForgetService services)
        {
            _services = services;
        }

        ///<summary>
        ///Forget Password with Random passwordgenerator that sends to user Email 
        ///</summary>


        [HttpPost]

        public async Task<ActionResult> ForgetPassword([FromBody] RandomPasswordEmail randompassword)

        {
            var randomstore = _services.ForgetPassword(randompassword.Email);

            return Ok(randomstore);

        }




    }
}
