using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<CourseCategory>> GetAllCategory();
        Task AddCategory(CourseCategory category);

        Task<bool> AnyCategoryByCategoryName(string Category);
        CourseCategory GetCategoryByCategoryId(Guid categoryId);
        CourseCategory GetCategoryByCategoryName(string categoryName);

    }
}
