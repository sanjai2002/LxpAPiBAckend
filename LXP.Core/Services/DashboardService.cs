using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public IEnumerable<DashboardLearnerViewModel> GetDashboardLearnerList()
        {
            return _dashboardRepository.GetTotalLearners();
        }

        IEnumerable<DashboardCourseViewModel> IDashboardService.GetDashboardCoursesList()
        {
            return _dashboardRepository.GetTotalCourses();
        }

        IEnumerable<DashboardEnrollmentViewModel> IDashboardService.GetDashboardEnrollmentList()
        {
            return _dashboardRepository.GetTotalEnrollments();
        }

        public Array GetMonthEnrollmentList()
        {
            var list = _dashboardRepository.GetMonthWiseEnrollments().ToList();
            var query = from c in list
                        group c by c.EnrollmentDate.Month into g
                        select new { EnrollMonth = g.Key, EnrollCount = g.Count() };
            Console.WriteLine(query);
            var output = query.ToList();
            return output.ToArray();
            //var groupedlist = from s in list
            //                  group s by s.EnrollmentDate.Month;
            //IEnumerable<string> groupedEnrollments = list
            //    .GroupBy(s=>s.EnrollmentDate.Month.ToString())
            //    .Select(grouping => string.Format("Type: {0} Count: {1}", grouping.Key, grouping.Count()));
            //Console.WriteLine(groupedEnrollments);
            //Console.WriteLine(groupedEnrollments.GetType());
            //Console.WriteLine(groupedlist);
        }

        public Array GetCourseCreatedList()
        {
            var list = _dashboardRepository.GetCourseCreated().ToList();
            var query = from c in list
                        group c by c.CreatedAt.Year into g
                        select new { CreatedYear = g.Key, CourseCount = g.Count() };
            Console.WriteLine(query);
            var output = query.ToList();
            return output.ToArray();
        }

        public string GetMostEnrolledCourse()
        {
            var course = _dashboardRepository.GetMoreEnrolledCourse();
            return "hi";
        }

        //IEnumerable<DashboardEnrollmentViewModel> IDashboardService.GetEnrollments()
        //{
        //    var result = _dashboardRepository.GetTotalEnrollments();
        //    return result;
        //}


        public AdminDashboardViewModel GetAdminDashboardDetails()
        {
            var AdminDashboard = new AdminDashboardViewModel
            {
                NoOfLearners = _dashboardRepository.GetNoOfLearners().Count(),
                NoOfCourse = _dashboardRepository.GetNoOfCourse().Count(),
                NoOfActiveLearners = _dashboardRepository.GetNoOfActiveLearners().Count(),
                HighestEnrolledCourse = _dashboardRepository.GetHighestEnrolledCourse(),
                GetTopLearners = _dashboardRepository.GetTopLearners(),
                GetTopFeedback = _dashboardRepository.GetFeedbackresponses(),
            };
            return AdminDashboard;

        }

    }
}
