using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ICategoryRepository
    {
        Task AddCategory(CourseCategory category);

        Task<List<CourseCategory>> GetAllCategory();

        Task<bool> AnyCategoryByCategoryName(string Category);
        CourseCategory GetCategoryByCategoryId(Guid categoryId);

    }
}