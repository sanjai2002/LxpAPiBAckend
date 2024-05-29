using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class QuizReportRepository : IQuizReportRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public QuizReportRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
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
                })
                .Select(q => new QuizReportViewModel
                {
                    CourseName = q.courseName,
                    TopicName = q.topicName,
                    QuizName = q.quizName,
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
                });

            return quizReports;
        }


    }
}

