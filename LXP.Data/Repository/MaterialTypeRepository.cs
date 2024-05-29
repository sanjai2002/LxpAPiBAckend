using LXP.Common.Entities;
using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public class MaterialTypeRepository : IMaterialTypeRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public MaterialTypeRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }
        public MaterialType GetMaterialTypeByMaterialTypeId(Guid materialTypeId)
        {
            return _lXPDbContext.MaterialTypes.Find(materialTypeId);
        }
        public List<MaterialType> GetAllMaterialTypes()
        {
            return _lXPDbContext.MaterialTypes.ToList();
        }

    }
}
