
using LXP.Common.Entities;
using LXP.Data;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LXP.Core.Repositories
{
    public class BulkQuestionRepository : IBulkQuestionRepository
    {
        private readonly LXPDbContext _dbContext;

        public BulkQuestionRepository(LXPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<QuizQuestion>> AddQuestionsAsync(List<QuizQuestion> questions)
        {
            await _dbContext.QuizQuestions.AddRangeAsync(questions);
            await _dbContext.SaveChangesAsync();
            return questions;
        }

        public async Task AddOptionsAsync(List<QuestionOption> questionOptions, Guid quizQuestionId)
        {
            var existingQuestion = await _dbContext.QuizQuestions.FirstOrDefaultAsync(q => q.QuizQuestionId == quizQuestionId);
            if (existingQuestion != null)
            {
                foreach (var option in questionOptions)
                {
                    option.QuizQuestionId = quizQuestionId;
                    _dbContext.QuestionOptions.Add(option);
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Quiz question with ID {quizQuestionId} does not exist.");
            }
        }

        public async Task<int> GetNextQuestionNoAsync(Guid quizId)
        {
            try
            {
                int count = await _dbContext.QuizQuestions
                    .Where(q => q.QuizId == quizId)
                    .CountAsync();

                return count + 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while retrieving the next question number asynchronously.",
                    ex
                );
            }
        }
    }
}



//public async Task<Quiz> GetQuizByNameAsync(string name)
//{
//    return await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.NameOfQuiz == name);
//}

//using LXP.Data.DBContexts;
//using LXP.Data.IRepository;
//using LXP.Data;
//namespace LXP.Core.Repositories
//{
//    public class BulkQuestionRepository : IBulkQuestionRepository
//    {
//        private readonly LXPDbContext _dbContext;

//        public BulkQuestionRepository(LXPDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public List<QuizQuestion> AddQuestions(List<QuizQuestion> questions)
//        {
//            _dbContext.QuizQuestions.AddRange(questions);
//            _dbContext.SaveChanges();
//            return questions;
//        }

//        public void AddOptions(List<QuestionOption> questionOptions, Guid quizQuestionId)
//        {
//            var existingQuestion = _dbContext.QuizQuestions.FirstOrDefault(q => q.QuizQuestionId == quizQuestionId);
//            if (existingQuestion != null)
//            {
//                foreach (var option in questionOptions)
//                {
//                    option.QuizQuestionId = quizQuestionId;
//                    _dbContext.QuizFeedbackQuestionOptions.Add(option);
//                }
//                _dbContext.SaveChanges();
//            }
//            else
//            {
//                throw new Exception($"Quiz question with ID {quizQuestionId} does not exist.");
//            }
//        }
//        public Quiz GetQuizByName(string name)
//        {
//            return _dbContext.Quizzes.FirstOrDefault(q => q.NameOfQuiz == quizName);
//        }
//    }
//}
//}