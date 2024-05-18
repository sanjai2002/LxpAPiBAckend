using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IDashboardService
    {
        IEnumerable<DashboardLearnerViewModel> GetDashboardLearnerList();
        IEnumerable<DashboardCourseViewModel> GetDashboardCoursesList();
        IEnumerable<DashboardEnrollmentViewModel> GetDashboardEnrollmentList();
        public Array GetMonthEnrollmentList();
        public Array GetCourseCreatedList();
        public string GetMostEnrolledCourse();

        public AdminDashboardViewModel GetAdminDashboardDetails();
    }
}