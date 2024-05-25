using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface ICategoryServices
    {
        Task<bool> AddCategory(CourseCategoryViewModel category);
        Task<CourseCategoryListViewModel> GetCategoryByCategoryName(string categoryName);

        Task<List<CourseCategoryListViewModel>> GetAllCategory();


    }
}
