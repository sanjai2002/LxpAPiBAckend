using LXP.Common.Entities;
using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly LXPDbContext _LXPDbContext;

        public QuizRepository(LXPDbContext dbContext)
        {
            _LXPDbContext = dbContext;
        }

        public void AddQuiz(Quiz quiz)
        {
            _LXPDbContext.Quizzes.Add(quiz);
            _LXPDbContext.SaveChanges();
        }

        public Quiz GetQuizById(Guid quizId)
        {
            return _LXPDbContext.Quizzes.Find(quizId);
        }

        public IEnumerable<Quiz> GetAllQuizzes()
        {
            return _LXPDbContext.Quizzes.ToList();
        }

        public void UpdateQuiz(Quiz quiz)
        {
            _LXPDbContext.Quizzes.Update(quiz);
            _LXPDbContext.SaveChanges();
        }

        public void DeleteQuiz(Quiz quiz)
        {
            _LXPDbContext.Quizzes.Remove(quiz);
            _LXPDbContext.SaveChanges();
        }

        public Topic GetTopicById(Guid topicId)
        {
            return _LXPDbContext.Topics.FirstOrDefault(t => t.TopicId == topicId);
        }

        public Quiz GetQuizByTopicId(Guid topicId)
        {
            return _LXPDbContext.Quizzes.FirstOrDefault(q => q.TopicId == topicId);
        }
    }
}



// using LXP.Data.IRepository;

// using LXP.Common.ViewModels.QuizViewModel;
// using LXP.Common.Entities;

// namespace LXP.Data.Repository
// {
//     public class QuizRepository : IQuizRepository
//     {
//         private readonly LXPDbContext _LXPDbContext;

//         public QuizRepository(LXPDbContext dbContext)
//         {
//             _LXPDbContext = dbContext;
//         }


//         public void CreateQuiz(QuizViewModel quiz, Guid topicId)
//         {
//             var topic = _LXPDbContext.Topics.FirstOrDefault(t => t.TopicId == topicId);
//             if (topic == null)
//                 throw new Exception($"Topic with id {topicId} not found.");

//             var courseId = topic.CourseId;

//             // Check if a quiz already exists for the given topic
//             var existingQuiz = _LXPDbContext.Quizzes.FirstOrDefault(q => q.TopicId == topicId);
//             if (existingQuiz != null)
//                 throw new Exception($"A quiz already exists for the topic with id {topicId}.");

//             if (string.IsNullOrWhiteSpace(quiz.NameOfQuiz))
//                 throw new Exception("NameOfQuiz cannot be null or empty.");
//             if (quiz.Duration <= 0)
//                 throw new Exception("Duration must be a positive integer.");
//             if (quiz.PassMark <= 0)
//                 throw new Exception("PassMark must be a positive integer.");
//             if (quiz.AttemptsAllowed.HasValue && quiz.AttemptsAllowed <= 0)
//                 throw new Exception("AttemptsAllowed must be null or a positive integer.");

//             var quizEntity = new Quiz
//             {
//                 QuizId = quiz.QuizId,
//                 CourseId = courseId,
//                 TopicId = topicId,
//                 NameOfQuiz = quiz.NameOfQuiz,
//                 Duration = quiz.Duration,
//                 PassMark = quiz.PassMark,
//                 AttemptsAllowed = quiz.AttemptsAllowed,
//                 CreatedBy = quiz.CreatedBy,
//                 CreatedAt = quiz.CreatedAt
//             };

//             _LXPDbContext.Quizzes.Add(quizEntity);
//             _LXPDbContext.SaveChanges();
//         }

//         public Guid? GetQuizIdByTopicId(Guid topicId)
//         {
//             var quizId = _LXPDbContext?.Quizzes.Where(q => q.TopicId == topicId).Select(q => q.QuizId).FirstOrDefault();
//             return quizId != Guid.Empty ? quizId : (Guid?)null;
//         }

//         public void UpdateQuiz(QuizViewModel quiz)
//         {
//             if (string.IsNullOrWhiteSpace(quiz.NameOfQuiz))
//                 throw new Exception("NameOfQuiz cannot be null or empty.");
//             if (quiz.Duration <= 0)
//                 throw new Exception("Duration must be a positive integer.");
//             if (quiz.PassMark <= 0)
//                 throw new Exception("PassMark must be a positive integer.");
//             if (quiz.AttemptsAllowed.HasValue && quiz.AttemptsAllowed <= 0)
//                 throw new Exception("AttemptsAllowed must be null or a positive integer.");

//             var quizEntity = _LXPDbContext.Quizzes.Find(quiz.QuizId);
//             if (quizEntity != null)
//             {
//                 quizEntity.NameOfQuiz = quiz.NameOfQuiz;
//                 quizEntity.Duration = quiz.Duration;
//                 quizEntity.PassMark = quiz.PassMark;
//                 quizEntity.AttemptsAllowed = quiz.AttemptsAllowed;

//                 _LXPDbContext.SaveChanges();
//             }
//         }

//         public void DeleteQuiz(Guid quizId)
//         {
//             var quizEntity = _LXPDbContext.Quizzes.Find(quizId);
//             if (quizEntity != null)
//             {
//                 _LXPDbContext.Quizzes.Remove(quizEntity);
//                 _LXPDbContext.SaveChanges();
//             }
//         }

//         public IEnumerable<QuizViewModel> GetAllQuizzes()
//         {
//             return _LXPDbContext.Quizzes
//                 .Select(q => new QuizViewModel
//                 {
//                     QuizId = q.QuizId,
//                     NameOfQuiz = q.NameOfQuiz,
//                     Duration = q.Duration,
//                     PassMark = q.PassMark,
//                     AttemptsAllowed = q.AttemptsAllowed
//                 })
//                 .ToList();
//         }

//         public QuizViewModel GetQuizById(Guid quizId)
//         {
//             var quiz = _LXPDbContext.Quizzes
//                 .Where(q => q.QuizId == quizId)
//                 .Select(q => new QuizViewModel
//                 {
//                     QuizId = q.QuizId,
//                     NameOfQuiz = q.NameOfQuiz,
//                     Duration = q.Duration,
//                     PassMark = q.PassMark,
//                     AttemptsAllowed = q.AttemptsAllowed
//                 })
//                 .FirstOrDefault();

//             if (quiz == null)
//             {
//                 return null; 
//             }

//             return quiz;
//         }
//     }
// }
