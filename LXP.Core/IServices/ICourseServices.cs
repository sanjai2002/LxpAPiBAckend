using LXP.Common.ViewModels;
using LXP.Common.Entities;

namespace LXP.Core.IServices
{
    public interface ICourseServices
    {
        Task<CourseListViewModel> GetCourseDetailsByCourseId(string courseId);
        Course GetCourseByCourseId(Guid courseId);


        CourseListViewModel GetCourseDetailsByCourseName(string courseName);
        CourseListViewModel AddCourse(CourseViewModel course);

        //Course GetCourseByCourseId(Guid courseId);

        Task<bool> Deletecourse(Guid courseId);

        Task<bool> Changecoursestatus(Coursestatus status);

        Task<bool> Updatecourse(CourseUpdateModel course);

        IEnumerable<CourseDetailsViewModel> GetAllCourse();
        IEnumerable<CourseDetailsViewModel> GetLimitedCourse();
    }
}