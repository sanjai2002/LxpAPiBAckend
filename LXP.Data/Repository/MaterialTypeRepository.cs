using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
