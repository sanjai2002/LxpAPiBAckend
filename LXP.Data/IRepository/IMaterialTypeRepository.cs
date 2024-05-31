using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface IMaterialTypeRepository
    {
        MaterialType GetMaterialTypeByMaterialTypeId(Guid materialTypeId);     // getting the material type using id
        List<MaterialType> GetAllMaterialTypes();     // get all material type available in the table
    }
}
