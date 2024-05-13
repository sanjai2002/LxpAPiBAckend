using LXP.Common;
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

    }
}
