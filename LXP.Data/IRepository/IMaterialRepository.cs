using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface IMaterialRepository
    {
        List<Material> GetAllMaterialDetailsByTopicAndType(Topic topic, MaterialType materialType);  // get
        Task AddMaterial(Material material);    //add
        Task<bool> AnyMaterialByMaterialNameAndTopic(string materialName,Topic topic);   
        Task<Material> GetMaterialByMaterialNameAndTopic(string materialName, Topic topic);

    }
}
