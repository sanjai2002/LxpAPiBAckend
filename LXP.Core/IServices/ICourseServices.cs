
using LXP.Common.Entities;
using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface ICourseServices
    {
        // check simi
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
