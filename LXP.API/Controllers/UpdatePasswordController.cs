using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatePasswordController : ControllerBase
    {

        private readonly IUpdatePasswordService _services;

        public UpdatePasswordController(IUpdatePasswordService services)
        {
            _services = services;
        }


        ///<summary>
        ///Update Password once user use the Forgot Password operation
        ///</summary>


        [HttpPut]

        public ActionResult LeanerUpdatePassword([FromBody] UpdatePassword updatepassword)
        {
            var result= _services.UpdatePassword(updatepassword);

            return Ok (result);
        }

    }
}


