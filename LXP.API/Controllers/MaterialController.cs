using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LXP.Common.ViewModels;



using LXP.Common.Constants;
using System.Net;
using LXP.Common.Entities;
namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : BaseController
    {
        private readonly IMaterialServices _materialService;
        public MaterialController(IMaterialServices materialService)
        {
            _materialService = materialService;
        }




        [HttpPost("/lxp/course/material")]
        public async Task<IActionResult> AddMaterial([FromForm]MaterialViewModel material)
        {
            MaterialListViewModel createdMaterial = await _materialService.AddMaterial(material);
            if (createdMaterial!=null)
            {
                return Ok(CreateInsertResponse(createdMaterial));
            }
            else
            {
                return Ok(CreateFailureResponse(MessageConstants.MsgAlreadyExists, (int)HttpStatusCode.PreconditionFailed));

            }
        }



        [HttpGet("/lxp/course/topic/{topicId}/materialtype/{materialTypeId}/")]
        public async Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndMaterialType(string topicId,string materialTypeId)
        {
            return await _materialService.GetAllMaterialDetailsByTopicAndType(topicId,materialTypeId) ;
        }
    }
}
