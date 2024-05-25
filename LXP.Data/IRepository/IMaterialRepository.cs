using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IMaterialRepository
    {
        List<Material> GetAllMaterialDetailsByTopicAndType(Topic topic, MaterialType materialType);
        Task AddMaterial(Material material);
        Task<bool> AnyMaterialByMaterialNameAndTopic(string materialName, Topic topic);
        Task<Material> GetMaterialByMaterialNameAndTopic(string materialName, Topic topic);

    }
}
