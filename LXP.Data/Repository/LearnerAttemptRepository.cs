using LXP.Common.Entities;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class  LearnerAttemptRepository : ILearnerAttemptRepository
    {
        private readonly LXPDbContext _dbcontext;


        public LearnerAttemptRepository(LXPDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public object GetScoreByTopicIdAndLernerId(Guid topicId, Guid LearnerId)
        {

            var result = from attempt in _dbcontext.LearnerAttempts
                         join quiz in _dbcontext.Quizzes on attempt.QuizId equals quiz.QuizId
                         join topic in _dbcontext.Topics on quiz.TopicId equals topic.TopicId
                         join course in _dbcontext.Courses on topic.CourseId equals course.CourseId
                         where attempt.LearnerId == LearnerId &&
                               topic.TopicId == topicId
                         select new
                         {
                             Score=attempt.Score,
                             CourseTitle=course.Title,
                             LearnerId=attempt.LearnerId,
                             TopicName=topic.Name,
                             TopicId=topic.TopicId,
                             CourseId=course.CourseId,
                             PassMark = quiz.PassMark,
                             Createdat=quiz.CreatedAt

                         };

            return result;
        }


        public object GetScoreByLearnerId(Guid LearnerId)
        {
            var result = from attempt in _dbcontext.LearnerAttempts
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
