using LXP.Common;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using System.Data.Entity;
using LXP.Common.Entities;

namespace LXP.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public DashboardRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }

        public IEnumerable<DashboardCourseViewModel> GetTotalCourses()
        {
            return _lXPDbContext.Courses
                 .Select(x => new DashboardCourseViewModel
                 {
                     CourseId = x.CourseId,
                     Title = x.Title,
                 })
                 .ToList();
        }

        public IEnumerable<DashboardEnrollmentViewModel> GetTotalEnrollments()
        {
            return _lXPDbContext.Enrollments
                 .Select(x => new DashboardEnrollmentViewModel
                 {
                     EnrollmentId = x.EnrollmentId,
                     CourseId = x.CourseId,
                     LearnerId = x.LearnerId,
                     EnrollmentDate = x.EnrollmentDate,
                 })
                 .ToList();
        }

        public IEnumerable<DashboardLearnerViewModel> GetTotalLearners()
        {
            return _lXPDbContext.Learners
                 .Select(x => new DashboardLearnerViewModel
                 {
                     LearnerId = x.LearnerId,
                     Email = x.Email,
                     Role = x.Role,
                 })
                 .Where(x => x.Role != "Admin")
                 .ToList();
        }

        public IEnumerable<DashboardEnrollmentViewModel> GetMonthWiseEnrollments()
        {
            //DateOnly StartDate = '20-01-2010';
            //DateOnly EndDate = DateOnly.FromDateTime(DateTime.Now);
            return _lXPDbContext.Enrollments
                .Select(x => new DashboardEnrollmentViewModel
                {
                    EnrollmentId = x.EnrollmentId,
                    CourseId = x.CourseId,
                    LearnerId = x.LearnerId,
                    EnrollmentDate = x.EnrollmentDate,
                })
                .ToList();
        }

        public IEnumerable<DashboardCourseViewModel> GetCourseCreated()
        {
            return _lXPDbContext.Courses
                .Select(x => new DashboardCourseViewModel
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                })
                .ToList();
        }

        public IEnumerable<DashboardEnrollmentViewModel> GetMoreEnrolledCourse()
        {
            return _lXPDbContext.Enrollments
                .Select(x => new DashboardEnrollmentViewModel
                {
                    EnrollmentId = x.EnrollmentId,
                    CourseId = x.CourseId,
                    LearnerId = x.LearnerId,
                    EnrollmentDate = x.EnrollmentDate,
                })
                .ToList();
        }


        public List<Learner> GetNoOfLearners()
        {
            return _lXPDbContext.Learners.Where(Learner => Learner.Role != "Admin").ToList();
        }

        public List<Course> GetNoOfCourse()
        {
            return _lXPDbContext.Courses.ToList();
        }


        public List<Learner> GetNoOfActiveLearners()
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);
            return _lXPDbContext.Learners.Where(Learner => Learner.Role!= "Admin" && Learner.UserLastLogin > OneMonthAgo).ToList();
        }

        public List<string> GetTopLearners()
        {
            var topLearners = _lXPDbContext.Enrollments
               .GroupBy(e => e.LearnerId)
               .OrderByDescending(g => g.Count())
               .Take(3)
               .Select(g => g.Key)
               .ToList();
            var topLearnersWithNames = _lXPDbContext.LearnerProfiles
                .Where(p => topLearners.Contains(p.LearnerId))
                 .Select(p => new { p.FirstName, p.LastName })
                   .ToList()
                 .Select(p => ($"{p.FirstName} {p.LastName}"))
                .ToList();

            return topLearnersWithNames;
        }

        public List<string> GetHighestEnrolledCourse()
        {
            var TopEnrolledCourses = _lXPDbContext.Enrollments
                .GroupBy(e => e.CourseId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { CourseId = g.Key, Count = g.Count() })
                .Take(3)
                .ToList();
            var courseNames = new List<string>();
            foreach (var course in TopEnrolledCourses)
            {
                var courseName = _lXPDbContext.Courses
                    .Where(c => c.CourseId == course.CourseId)
                    .Select(c => c.Title)
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(courseName))
                {
                    courseNames.Add(courseName);
                }
            }
            return courseNames;
        }

        public List<string> GetFeedbackresponses()
        {
            var feedbackResponses = _lXPDbContext.Feedbackresponses
          .OrderByDescending(e => e.GeneratedAt)
          .Where(p=>p.Response!=null)
          .Select(p => p.Response)// Select only the 'Response' property
          .Take(3)
          .ToList(); // Convert the result to a list of strings

            return feedbackResponses;
        }

    }
}
