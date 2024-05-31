using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IBulkQuestionRepository
    {
        Task<List<QuizQuestion>> AddQuestionsAsync(List<QuizQuestion> quizQuestions);
        Task AddOptionsAsync(List<QuestionOption> questionOptions, Guid quizQuestionId);
       
        Task<int> GetNextQuestionNoAsync(Guid quizId);
        
    }
}




// Task<Quiz> GetQuizByNameAsync(string name);

//using System.Threading.Tasks;
//using LXP.Common.DTO;
//using Microsoft.AspNetCore.Http;

//namespace LXP.Data.IRepository
//{
//    public interface IBulkQuestionRepository
//    {
//        List<QuizQuestion> AddQuestions(List<QuizQuestion> quizQuestions);
//        void AddOptions(List<QuestionOption> questionOptions, Guid quizQuestionId);

//        Quiz GetQuizByName(string name);
//        // Add other repository methods as needed
//    }
//}
// Add other repository methods as needed
