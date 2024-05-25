using LXP.Common.Entities;
namespace LXP.Data.IRepository
{
    public interface ICourseLevelRepository
    {
        Task AddCourseLevel(CourseLevel level);

        Task<List<CourseLevel>> GetAllCourseLevel();

        CourseLevel GetCourseLevelByCourseLevelId(Guid courseLevelId);

    }
}
