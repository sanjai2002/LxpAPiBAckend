using LXP.Common.Entities;
using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public class LearnerAttemptRepository : ILearnerAttemptRepository
    {
        private readonly LXPDbContext _dbcontext;

        public LearnerAttemptRepository(LXPDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public object GetScoreByTopicIdAndLernerId(Guid LearnerId)
        {
                var result =
                 from attempt in _dbcontext.LearnerAttempts
             
                 join quiz in _dbcontext.Quizzes on attempt.QuizId equals quiz.QuizId
             
                 join topic in _dbcontext.Topics on quiz.TopicId equals topic.TopicId
             
                 join course in _dbcontext.Courses on topic.CourseId equals course.CourseId
             
                 where attempt.LearnerId == LearnerId
             
                 group attempt.Score by new
                 {
                     quiz.QuizId,
             
                     CourseName = course.Title,
             
                     attempt.LearnerId,
             
                     TopicName = topic.Name,
             
                     topic.TopicId,
             
                     course.CourseId,
                     quiz.PassMark
                 } into grouped
             
                 select new
                 {
                    Score = grouped.Max(),
             
                     grouped.Key.QuizId,
             
                     grouped.Key.TopicName,
             
                     grouped.Key.LearnerId,
             
                     grouped.Key.CourseName,
             
                     grouped.Key.TopicId,
             
                     grouped.Key.CourseId,
                     grouped.Key.PassMark
                 };
            result = result.Where(r => r.Score >= r.PassMark);
             
            return result;
        }

        public object GetScoreByLearnerId(Guid LearnerId)
        {
            var result =
                from attempt in _dbcontext.LearnerAttempts
                join quiz in _dbcontext.Quizzes on attempt.QuizId equals quiz.QuizId
                join topic in _dbcontext.Topics on quiz.TopicId equals topic.TopicId
                join course in _dbcontext.Courses on topic.CourseId equals course.CourseId
                where attempt.LearnerId == LearnerId
                group attempt by course into groupedScores
                select new
                {
                    CourseId = groupedScores.Key.CourseId,
                    CourseName = groupedScores.Key.Title,
                    TotalScore = groupedScores.Sum(attempt => attempt.Score)
                };
            return result;
        }
    }
}
