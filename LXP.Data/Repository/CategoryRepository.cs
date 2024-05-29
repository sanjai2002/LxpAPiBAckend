using LXP.Common.Entities;

using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LXP.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public CategoryRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }

        public async Task<List<CourseCategory>> GetAllCategory()
        {
            return await _lXPDbContext.CourseCategories.ToListAsync();
        }
        public async Task AddCategory(CourseCategory category)
        {
            await _lXPDbContext.CourseCategories.AddAsync(category);
            await _lXPDbContext.SaveChangesAsync();
        }
        public async Task<bool> AnyCategoryByCategoryName(string Category)
        {
            return await _lXPDbContext.CourseCategories.AnyAsync(category => category.Category == Category);
        }
        public CourseCategory GetCategoryByCategoryId(Guid categoryId)
        {
            return _lXPDbContext.CourseCategories.Find(categoryId);
        }
        public CourseCategory GetCategoryByCategoryName(string categoryName)
        {
            return _lXPDbContext.CourseCategories.First(category => category.Category == categoryName);
        }


    }
}
