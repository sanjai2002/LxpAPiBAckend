using LXP.Common.Entities;
using LXP.Data.IRepository;

namespace LXP.Data.IRepository
{
    public class CourseLevelRepository : ICourseLevelRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public CourseLevelRepository(LXPDbContext lXPDbContext)
        {
            this._lXPDbContext = lXPDbContext;
        }

        public async Task AddCourseLevel(CourseLevel level)
        {
            await _lXPDbContext.CourseLevels.AddAsync(level);
            await _lXPDbContext.SaveChangesAsync();
        }

        public async Task<List<CourseLevel>> GetAllCourseLevel()
        {
            return _lXPDbContext.CourseLevels.ToList();
        }
        public CourseLevel GetCourseLevelByCourseLevelId(Guid courseLevelId)
        {
            return _lXPDbContext.CourseLevels.Find(courseLevelId);
        }
    }

}
