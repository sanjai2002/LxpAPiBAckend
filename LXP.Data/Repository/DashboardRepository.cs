using System.Data.Entity;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LXP.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public DashboardRepository(
            LXPDbContext lXPDbContext,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _lXPDbContext = lXPDbContext;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }

        public IEnumerable<DashboardCourseViewModel> GetTotalCourses()
        {
            return _lXPDbContext
                .Courses.Select(x => new DashboardCourseViewModel
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                })
                .ToList();
        }

        public IEnumerable<DashboardEnrollmentViewModel> GetTotalEnrollments()
        {
            return _lXPDbContext
                .Enrollments.Select(x => new DashboardEnrollmentViewModel
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
            return _lXPDbContext
                .Learners.Select(x => new DashboardLearnerViewModel
                {
                    LearnerId = x.LearnerId,
                    Email = x.Email,
                    Role = x.Role,
                })
                .Where(x => x.Role != "Admin")
                .ToList();
        }

        public IEnumerable<DashboardEnrollmentViewModel> GetMonthWiseEnrollments(string year)
        {
            //DateOnly StartDate = '20-01-2010';
            //DateOnly EndDate = DateOnly.FromDateTime(DateTime.Now);

            return _lXPDbContext
                .Enrollments.Select(x => new DashboardEnrollmentViewModel
                {
                    EnrollmentId = x.EnrollmentId,
                    CourseId = x.CourseId,
                    LearnerId = x.LearnerId,
                    EnrollmentDate = x.EnrollmentDate,
                })
                .Where(x => x.EnrollmentDate.Year.ToString() == year)
                .ToList();
        }

        public IEnumerable<DashboardCourseViewModel> GetCourseCreated()
        {
            return _lXPDbContext
                .Courses.Select(x => new DashboardCourseViewModel
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                })
                .ToList();
        }

        public IEnumerable<DashboardEnrollmentViewModel> GetMoreEnrolledCourse()
        {
            return _lXPDbContext
                .Enrollments.Select(x => new DashboardEnrollmentViewModel
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
            return _lXPDbContext
                .Learners.Where(Learner =>
                    Learner.Role != "Admin" && Learner.UserLastLogin > OneMonthAgo
                )
                .ToList();
        }

        public List<string> GetTopLearners()
        {
            var topLearners = _lXPDbContext
                .Enrollments.GroupBy(e => e.LearnerId)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();
            var topLearnersWithNames = _lXPDbContext
                .LearnerProfiles.Where(p => topLearners.Contains(p.LearnerId))
                .Select(p => new { p.FirstName, p.LastName })
                .ToList()
                .Select(p => ($"{p.FirstName} {p.LastName}"))
                .ToList();

            return topLearnersWithNames;
        }

        public List<string> GetFeedbackresponses()
        {
            var feedbackResponses = _lXPDbContext
                .Feedbackresponses.OrderByDescending(e => e.GeneratedAt)
                .Where(p => p.Response != null)
                .Select(p => p.Response) // Select only the 'Response' property
                .Take(3)
                .ToList(); // Convert the result to a list of strings

            return feedbackResponses;
        }

        public IEnumerable<TopLearnersViewModel> GetTopLearner()
        {
            var topLearners = _lXPDbContext
                .Enrollments.GroupBy(e => e.LearnerId)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => new TopLearnersViewModel
                {
                    Learnerid = g.Key,
                    LearnerName =
                        g.First().Learner.LearnerProfiles.First().FirstName
                        + " "
                        + g.First().Learner.LearnerProfiles.First().LastName,
                    ProfilePhoto = String.Format(
                        "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                        _contextAccessor.HttpContext.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        g.First().Learner.LearnerProfiles.First().ProfilePhoto
                    )
                })
                .ToList();
            return topLearners;
        }

        public IEnumerable<HighestEnrolledCourseViewModel> GetHighestEnrolledCourse()
        {
            var HighestEnrolledCourses = _lXPDbContext
                .Enrollments.GroupBy(e => e.CourseId)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => new HighestEnrolledCourseViewModel
                {
                    Courseid = g.Key,
                    CourseName = g.First().Course.Title,
                    Thumbnailimage = String.Format(
                        "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                        _contextAccessor.HttpContext.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        g.First().Course.Thumbnail
                    ),
                    Learnerscount = g.Count(),
                })
                .ToList();
            return HighestEnrolledCourses;
        }

        public IEnumerable<RecentFeedbackViewModel> GetRecentfeedbackResponses()
        {
            var RecentfeedbackResponses = _lXPDbContext
                .Feedbackresponses.OrderByDescending(e => e.GeneratedAt)
                .Where(p => p.Response != "")
                .Take(3)
                .Select(g => new RecentFeedbackViewModel
                {
                    Feedbackresponse = g.Response,
                    Topicfeedbackquestions = g.TopicFeedbackQuestion!.Question,
                    FeedbackresponseId = g.FeedbackresponseId,
                    DateoftheResponse = (DateTime)g.GeneratedAt!,
                    TopicName = g.TopicFeedbackQuestion!.Topic.Name,
                    Coursename = g.TopicFeedbackQuestion.Topic.Course.Title,
                    Learnerid = g.LearnerId,
                    LearnerName =
                        g.Learner.LearnerProfiles.First().FirstName
                        + " "
                        + g.Learner.LearnerProfiles.First().LastName,
                    Profilephoto = String.Format(
                        "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                        _contextAccessor.HttpContext.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        g.Learner.LearnerProfiles.First().ProfilePhoto
                    )
                })
                .ToList();
            return RecentfeedbackResponses;
        }

        public List<string> GetEnrolledYears()
        {
            var years = _lXPDbContext
                .Enrollments.Select(p => p.EnrollmentDate.Year.ToString())
                .Distinct()
                .ToList();
            return years;
        }

        public List<Learner> GetNoOfInActiveLearners()
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);
            return _lXPDbContext
                .Learners.Where(Learner =>
                    Learner.Role != "Admin" && Learner.UserLastLogin < OneMonthAgo
                )
                .ToList();
        }

        public IEnumerable<CourseWiseViewModel> GetEnrollmentCourse()
        {
            var topLearners = _lXPDbContext
                .Enrollments.GroupBy(e => e.CourseId)
                .Select(g => new CourseWiseViewModel
                {
                    CourseId = g.Key,
                    Count = g.Count(),
                    CourseName = g.First().Course.Title,
                })
                .ToList();
            foreach (var learner in topLearners)
            {
                Console.WriteLine("hi" + learner.CourseId);
            }
            ;
            return topLearners;
        }
    }
}
