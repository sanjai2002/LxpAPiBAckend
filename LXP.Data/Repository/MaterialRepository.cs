using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public MaterialRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }
        public async Task AddMaterial(Material material)
        {
           await _lXPDbContext.Materials.AddAsync(material);
           await _lXPDbContext.SaveChangesAsync();
                 
        }

        public async Task<bool> AnyMaterialByMaterialNameAndTopic(string materialName,Topic topic)
        {
            return await _lXPDbContext.Materials.AnyAsync(material=>material.Name==materialName&&material.Topic==topic);
        }
        public async Task<Material> GetMaterialByMaterialNameAndTopic(string materialName, Topic topic)
        {
            return await _lXPDbContext.Materials.FirstOrDefaultAsync(material => material.Name == materialName && material.Topic == topic);
        }


        public List<Material> GetAllMaterialDetailsByTopicAndType(Topic topic, MaterialType materialType)
        {
            return _lXPDbContext.Materials.Include(material=>material.MaterialType).Include(material=>material.Topic).Where(material=>material.IsActive==true&&material.Topic==topic&& material.MaterialType==materialType).Include(material => material.Topic).Include(material => material.MaterialType).ToList();

        }

        

        

      


    }
}
