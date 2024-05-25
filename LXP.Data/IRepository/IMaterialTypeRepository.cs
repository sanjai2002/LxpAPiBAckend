using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IMaterialTypeRepository
    {
        MaterialType GetMaterialTypeByMaterialTypeId(Guid materialTypeId);
        List<MaterialType> GetAllMaterialTypes();
    }
}
