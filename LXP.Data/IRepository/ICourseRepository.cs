
using LXP.Common.Entities;
using LXP.Common.ViewModels;
namespace LXP.Data.IRepository
{
    public interface ICourseRepository
    {
        void AddCourse(Course course);
        bool AnyCourseByCourseTitle(string courseTitle);
        Course GetCourseDetailsByCourseName(string courseName);




        Course GetCourseDetailsByCourseId(Guid CourseId);

        Course FindCourseid(Guid courseid);

        public Enrollment FindEntrollmentcourse(Guid Courseid);

        Task Deletecourse(Course course);

        Task Changecoursestatus(Course course);

        Task Updatecourse(Course course);

        IEnumerable<CourseDetailsViewModel> GetAllCourse();
        IEnumerable<CourseDetailsViewModel> GetLimitedCourse();


    }
}
