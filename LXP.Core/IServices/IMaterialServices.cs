using LXP.Common.ViewModels;


namespace LXP.Core.IServices
{
    public interface IMaterialServices
    {
        Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndType(string topicId, string materialTypeId);
        Task<MaterialListViewModel> AddMaterial(MaterialViewModel material);
        Task<MaterialListViewModel> GetMaterialByMaterialNameAndTopic(string materialName, string topicId);

    }
}
