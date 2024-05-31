
using LXP.Common.ViewModels.QuizViewModel;


namespace LXP.Core.IServices
{
    public interface IQuizService
    {
        QuizViewModel GetQuizById(Guid quizId);
        IEnumerable<QuizViewModel> GetAllQuizzes();
        void CreateQuiz(QuizViewModel quiz, Guid TopicId);
        void UpdateQuiz(QuizViewModel quiz);
        void DeleteQuiz(Guid quizId);
        Guid? GetQuizIdByTopicId(Guid topicId);

    }
}
//using LXP.Common.DTO;
//using System;
//using System.Collections.Generic;

//namespace LXP.Core.IServices
//{
//    public interface IQuizService
//    {
//        QuizDto GetQuizById(Guid quizId);
//        IEnumerable<QuizDto> GetAllQuizzes();
//        void CreateQuiz(QuizDto quiz, Guid TopicId);
//        void UpdateQuiz(QuizDto quiz);
//        void DeleteQuiz(Guid quizId);
//    }
//}

////using LXP.Common.DTO;
////using System;
////using System.Collections.Generic;
////using System.Threading.Tasks;

////namespace LXP.Core.IServices
////{
////    public interface IQuizService
////    {
////        Task<QuizDto> GetQuizByIdAsync(Guid quizId);
////        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
////        Task UpdateQuizAsync(QuizDto quiz);
////        Task DeleteQuizAsync(Guid quizId);
////        Task CreateQuizAsync(QuizDto quiz, Guid TopicId);
////    }
////}


//////using LXP.Common.DTO;
//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Text;
//////using System.Threading.Tasks;

//////namespace LXP.Core.IServices
//////{
//////    public interface IQuizService
//////    {
//////        QuizDto GetQuizById(Guid quizId);
//////        IEnumerable<QuizDto> GetAllQuizzes();

//////        void UpdateQuiz(QuizDto quiz);
//////        void DeleteQuiz(Guid quizId);
//////        void CreateQuiz(QuizDto quiz, Guid TopicId);
//////    }
//////}

////////void CreateQuiz(Guid quizId, Guid courseId, Guid topicId, string nameOfQuiz, int duration, int passMark, string createdBy, DateTime createdAt);

//////// void CreateQuiz(QuizDto quiz);