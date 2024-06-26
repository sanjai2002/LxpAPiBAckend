using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IDashboardService
    {
        IEnumerable<DashboardLearnerViewModel> GetDashboardLearnerList();
        IEnumerable<DashboardCourseViewModel> GetDashboardCoursesList();
        IEnumerable<DashboardEnrollmentViewModel> GetDashboardEnrollmentList();
        public Array GetMonthEnrollmentList(string year);
        public Array GetCourseCreatedList();
        public string GetMostEnrolledCourse();

        public AdminDashboardViewModel GetAdminDashboardDetails();
        IEnumerable<TopLearnersViewModel> GetTopLearner();
        IEnumerable<HighestEnrolledCourseViewModel> GetHighestEnrolledCourse();
        IEnumerable<RecentFeedbackViewModel> GetRecentfeedbackResponses();
        public Array GetCourseWiseEnrollmentsCount();
    }
}
