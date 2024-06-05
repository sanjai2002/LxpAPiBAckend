using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LXP.Data.Repository
{
    public class QuizReportRepository : IQuizReportRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public QuizReportRepository(LXPDbContext lXPDbContext, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _lXPDbContext = lXPDbContext;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }

 
        public IEnumerable<QuizReportViewModel> GetQuizReports()
        {
            var quizReports = _lXPDbContext.Quizzes
                .Select(q => new
                {
                    courseName = q.Course.Title,
                    topicName = q.Topic.Name,
                    quizName = q.NameOfQuiz,
                    QuizId = q.QuizId,
                    PassMark = q.PassMark,
                    LearnerId = q.LearnerAttempts.First().LearnerId,
                })
                .Select(q => new QuizReportViewModel
                {
                    CourseName = q.courseName,
                    TopicName = q.topicName,
                    QuizName = q.quizName,
                    QuizId = q.QuizId,
                    NoOfPassedUsers = _lXPDbContext.LearnerAttempts
                        .Where(attempt => attempt.QuizId == q.QuizId)
                        .GroupBy(attempt => attempt.LearnerId)
                        .Count(group => group.Max(attempt => attempt.Score) >= q.PassMark),
                    NoOfFailedUsers = _lXPDbContext.LearnerAttempts
                        .Where(attempt => attempt.QuizId == q.QuizId)
                        .GroupBy(attempt => attempt.LearnerId)
                        .Count(group => group.Max(attempt => attempt.Score) < q.PassMark),
                    AverageScore = _lXPDbContext.LearnerAttempts
                        .Where(attempt => attempt.QuizId == q.QuizId)
                        .GroupBy(attempt => attempt.LearnerId)
                        .Select(group => group.Max(attempt => attempt.Score))
                        .DefaultIfEmpty()
                        .Average()
                }); ;

            return quizReports;
        }
        public IEnumerable<QuizScorelearnerViewModel> GetPassdLearnersList(Guid Quizid)
        {
            var quiz = _lXPDbContext.Quizzes.Find(Quizid);
            var attempts = _lXPDbContext.LearnerAttempts
               .Where(e => quiz!.QuizId.Equals(e.QuizId) && e.Score >= quiz.PassMark)
                .GroupBy(m => m.LearnerId)
                .Select(m => new QuizScorelearnerViewModel
                {
                    LearnerId = m.Key,
                    LearnerAttempts = m.Max(e => e.AttemptCount),
                    LearnerName = m.First().Learner.LearnerProfiles.First().FirstName + " " + m.First().Learner.LearnerProfiles.First().LastName,
                    Score = m.Max(e => e.Score),
                    TotalNoofQuizAttempts = (int)quiz!.AttemptsAllowed!,
                    Profilephoto = String.Format("{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",

                                           _contextAccessor.HttpContext.Request.Scheme,
                                           _contextAccessor.HttpContext.Request.Host,
                                           _contextAccessor.HttpContext.Request.PathBase, m.First().Learner.LearnerProfiles.First().ProfilePhoto)
                })
                .ToList();
            return attempts;
        }

        public IEnumerable<QuizScorelearnerViewModel> GetFailedLearnersList(Guid Quizid)
        {
            var quiz = _lXPDbContext.Quizzes.Find(Quizid);
            var attempts = _lXPDbContext.LearnerAttempts
               .Where(e => quiz!.QuizId.Equals(e.QuizId) && e.Score <= quiz.PassMark)
                .GroupBy(m => m.LearnerId)
                .Select(m => new QuizScorelearnerViewModel
                {
                    LearnerId = m.Key,
                    LearnerAttempts = m.Max(e => e.AttemptCount),
                    LearnerName = m.First().Learner.LearnerProfiles.First().FirstName + " " + m.First().Learner.LearnerProfiles.First().LastName,
                    Score = m.Max(e => e.Score),
                    TotalNoofQuizAttempts = (int)quiz!.AttemptsAllowed!,
                    Profilephoto = String.Format("{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                                           _contextAccessor.HttpContext.Request.Scheme,
                                           _contextAccessor.HttpContext.Request.Host,
                                           _contextAccessor.HttpContext.Request.PathBase, m.First().Learner.LearnerProfiles.First().ProfilePhoto)
                })
                .ToList();
            return attempts;
        }

    }
}

