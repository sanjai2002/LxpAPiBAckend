using LXP.Common.ViewModels;
using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LXP.Core.IServices
{
    public interface IMaterialServices
    {
      
        Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndType(string topicId, string materialTypeId); // get
        Task<MaterialListViewModel> AddMaterial(MaterialViewModel material);
        Task<MaterialListViewModel> GetMaterialByMaterialNameAndTopic(string materialName, string topicId);
        Task<bool> SoftDeleteMaterial(string materialId);
        Task<bool> UpdateMaterial(MaterialUpdateViewModel material);

        Task<MaterialListViewModel> GetMaterialDetailsByMaterialId(string materialId);

        Task<MaterialListViewModel> GetMaterialDetailsByMaterialIdWithoutPDFConversionForUpdate(string materialId);

    }
}
