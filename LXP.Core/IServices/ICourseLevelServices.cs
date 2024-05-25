
using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface ICourseLevelServices
    {

        Task<List<CourseLevelListViewModel>> GetAllCourseLevel(string CreatedBy);
        Task AddCourseLevel(string Level, string CreatedBy);

    }
}
