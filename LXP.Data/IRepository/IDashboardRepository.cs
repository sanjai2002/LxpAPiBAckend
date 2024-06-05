
using LXP.Common.Entities;
using LXP.Common.ViewModels;
namespace LXP.Data.IRepository
{
    public interface IDashboardRepository
    {
        IEnumerable<DashboardLearnerViewModel> GetTotalLearners();
        IEnumerable<DashboardCourseViewModel> GetTotalCourses();
        IEnumerable<DashboardEnrollmentViewModel> GetTotalEnrollments();
        IEnumerable<DashboardEnrollmentViewModel> GetMonthWiseEnrollments(string year);
        IEnumerable<DashboardCourseViewModel> GetCourseCreated();
        IEnumerable<DashboardEnrollmentViewModel> GetMoreEnrolledCourse();
        public List<Learner> GetNoOfLearners();
        public List<Course> GetNoOfCourse();
        public List<Learner> GetNoOfActiveLearners();
        public List<Learner> GetNoOfInActiveLearners();
        public List<string> GetTopLearners();
        public List<string> GetFeedbackresponses();
        IEnumerable<TopLearnersViewModel> GetTopLearner();
        IEnumerable<HighestEnrolledCourseViewModel> GetHighestEnrolledCourse();
        IEnumerable<RecentFeedbackViewModel> GetRecentfeedbackResponses();
        public List<string> GetEnrolledYears();
    }
}
