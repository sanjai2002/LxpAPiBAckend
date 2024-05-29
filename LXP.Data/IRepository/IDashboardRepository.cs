
using LXP.Common.Entities;
using LXP.Common.ViewModels;
namespace LXP.Data.IRepository
{
    public interface IDashboardRepository
    {
        IEnumerable<DashboardLearnerViewModel> GetTotalLearners();
        IEnumerable<DashboardCourseViewModel> GetTotalCourses();
        IEnumerable<DashboardEnrollmentViewModel> GetTotalEnrollments();
        IEnumerable<DashboardEnrollmentViewModel> GetMonthWiseEnrollments();
        IEnumerable<DashboardCourseViewModel> GetCourseCreated();
        IEnumerable<DashboardEnrollmentViewModel> GetMoreEnrolledCourse();


        public List<Learner> GetNoOfLearners();
        public List<Course> GetNoOfCourse();
        public List<Learner> GetNoOfActiveLearners();
        public List<string> GetHighestEnrolledCourse();

        public List<string> GetTopLearners();
        public List<string> GetFeedbackresponses();
    }
}
