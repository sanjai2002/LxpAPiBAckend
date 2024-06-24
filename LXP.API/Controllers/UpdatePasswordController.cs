using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatePasswordController : BaseController
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
        public async Task<IActionResult> LeanerUpdatePassword(
            [FromBody] UpdatePassword updatepassword
        )
        {
            bool result = await _services.UpdatePassword(updatepassword);
            if (result)
            {
                return Ok("Password Updated Successfully");
            }
            else
            {
                return Ok("Incorrect Received Password");
            }
        }
    }
}
