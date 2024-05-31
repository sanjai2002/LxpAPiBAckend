
using LXP.Common.ViewModels.QuizViewModel;


namespace LXP.Data.IRepository
{
    public interface IQuizRepository
    {
        QuizViewModel GetQuizById(Guid quizId);
        IEnumerable<QuizViewModel> GetAllQuizzes();
        void CreateQuiz(QuizViewModel quiz, Guid TopicId);
        void UpdateQuiz(QuizViewModel quiz);
        void DeleteQuiz(Guid quizId);
      
        Guid? GetQuizIdByTopicId(Guid topicId);
    }
}
// Task<Quiz> GetQuizByNameAsync(string name);
//using LXP.Common.DTO;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace LXP.Data.IRepository
//{
//    public interface IQuizRepository
//    {
//        Task<QuizDto> GetQuizByIdAsync(Guid quizId);
//        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
//        Task UpdateQuizAsync(QuizDto quiz);
//        Task DeleteQuizAsync(Guid quizId);
//        Task CreateQuizAsync(QuizDto quiz, Guid TopicId);
//    }
//}

////using LXP.Common.DTO;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace LXP.Data.IRepository
////{
////    public interface IQuizRepository
////    {

////        QuizDto GetQuizById(Guid quizId);
////        IEnumerable<QuizDto> GetAllQuizzes();


////        void UpdateQuiz(QuizDto quiz);
////        void DeleteQuiz(Guid quizId);
////        Task<Quiz> GetQuizByNameAsync(string name);
////        void CreateQuiz(QuizDto quiz, Guid TopicId);




////    }
////}

////  void CreateQuiz(QuizDto quiz);